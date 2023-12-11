using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;

namespace QuizMaster.API.Gateway.Services
{
    public class QuizHandler
    {
        private List<string> InSession;
        public QuizHandler()
        {
            InSession = new();
        }
        public async Task StartQuiz(SessionHub hub, SessionHandler handler, QuizRoomService.QuizRoomServiceClient grpcClient, string roomId)
        {
            // Check if the current session is started, otherwise don't proceed
            if (InSession.Contains(roomId))
            {
                await hub.Clients.Caller.SendAsync("notif", "Room has already started");
                return;
            }

            string roomPin = "";
            // Get the SetQuizRoom
            var set = new SetRequest() { Id = Convert.ToInt32(roomId) };
            var qsetReply = await grpcClient.GetQuizSetAsync(set);
            var quizSets = JsonConvert.DeserializeObject<List<SetQuizRoom>>(qsetReply.Data);

            if (quizSets == null) { await hub.Clients.Caller.SendAsync("notif", "Failed to start a room, SetQuizRoom is not available"); return; }
            if (quizSets.Count == 0)
            {
                await hub.Clients.Caller.SendAsync("notif", "Failed to start a room, There is no question sets available");
                return;
            }

            InSession.Add(roomId); // register the room to be started
            foreach (var Qset in quizSets)
            {
                // Get the Sets
                var setRequest = new SetRequest() { Id = Qset.QSetId };
                var setReply = await grpcClient.GetQuizAsync(setRequest);

                var Setquestions = JsonConvert.DeserializeObject<List<QuestionSet>>(setReply.Data);

                if (Setquestions == null) continue;


                foreach (var questionSet in Setquestions)
                {
                    roomPin = Qset.QRoom.QRoomPin + "";
                    //await hub.Clients.Group(roomPin).SendAsync("notif", "Displaying a question");
                    var questionRequest = new SetRequest() { Id = questionSet.QuestionId };
                    var questionSetReply = await grpcClient.GetQuestionAsync(questionRequest);

                    var details = JsonConvert.DeserializeObject<QuestionsDTO>(questionSetReply.Data);


                    if (details == null) continue;

                    handler.SetCurrentQuestionDisplayed(roomPin, details.question.Id);// Save the current displayed Question
                    var timout = details.question.QTime;

                    // setting the current set info
                    details.CurrentSetName = questionSet.Set.QSetName;
                    details.CurrentSetDesc = questionSet.Set.QSetDesc;

                    for (int time = timout; time >= 0; time--)
                    {
                        details.RemainingTime = time;
                        await hub.Clients.Group(roomPin).SendAsync("question", details);
                        await Task.Delay(1000);
                    }
                }
            }
            await hub.Clients.Group(roomPin).SendAsync("stop", "Quiz has ended, scores and sent");
            // send scores
            var participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            foreach (var participant in participants)
                await hub.Clients.Group(roomPin).SendAsync("notif", $"Score: {participant.Score} | Name: {participant.QParticipantDesc}");
            handler.RemoveRoomDataCurrentDisplayed(roomPin); // clear the last question
            InSession.Remove(roomId); // Quiz ended, can restart

            // TODO: Save the Participant's Scores and reset their data's

            // Clear
            handler.ResetParticipantLinkedConnectionsInAGroup(roomPin);
        }
    }
}
