using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;

namespace QuizMaster.API.Gateway.Services
{
    public class QuizHandler
    {
        private GrpcChannel _channel;
        private QuizRoomService.QuizRoomServiceClient _channelClient;
        private List<string> InSession;
        public QuizHandler(IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizRoomService.QuizRoomServiceClient(_channel);
            InSession = new();
        }
        public async Task StartQuiz(SessionHub hub, SessionHandler handler, QuizRoomService.QuizRoomServiceClient grpcClient, string roomId)
        {
            string hostConnectionId = hub.Context.ConnectionId;
            DateTime startTime = DateTime.UtcNow;
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
                    // update the participant start-time if haven't, for late joiners
                    UpdateParticipantsStartTime(handler, roomPin);
                    AcceptParticipantsAnswerSubmission(handler, roomPin, Qset.QRoom);
                    //await hub.Clients.Group(roomPin).SendAsync("notif", "Displaying a question");
                    var questionRequest = new SetRequest() { Id = questionSet.QuestionId };
                    var questionSetReply = await grpcClient.GetQuestionAsync(questionRequest);

                    var details = JsonConvert.DeserializeObject<QuestionsDTO>(questionSetReply.Data);


                    if (details == null) continue;

                    handler.SetCurrentQuestionDisplayed(roomPin, details.question.Id);// Save the current displayed Question
                    var timout = 5;//details.question.QTime;

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
            
            EndParticipantsStartTime(handler, roomPin); // End participant time
            await SendParticipantsScoresAsync(hub, handler, roomPin); // send scores
            handler.RemoveRoomDataCurrentDisplayed(roomPin); // clear the last question
            InSession.Remove(roomId); // Quiz ended, can restart

            // TODO: Save the Participant's Scores and reset their data's
            await SaveQuizRoomDataAsync(handler, hostConnectionId, Convert.ToInt32(roomId), roomPin, startTime, quizSets);
            // Clear
            handler.ResetParticipantLinkedConnectionsInAGroup(roomPin);
            await hub.Clients.Group(roomPin).SendAsync("notif", "Quiz Data: "+JsonConvert.SerializeObject(await GetQuizRoomDatasAsync()));
        }

        public void AcceptParticipantsAnswerSubmission(SessionHandler handler, string roomPin, QuizRoom room)
        {
            var participantsConnectionIds = handler.GetConnectionIdsInAGroup(roomPin);
            foreach (var participantConnectionId in participantsConnectionIds)
            {
                handler.RemoveHoldOnAnswerSubmission(participantConnectionId);
                var participant = handler.GetLinkedParticipantInConnectionId(participantConnectionId);
                if (participant != null) participant.QRoomId = room.Id;
            }
        }

        public void UpdateParticipantsStartTime(SessionHandler handler, string roomPin)
        {
            var participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            foreach (var participant in participants)
            {
                // Only update the participant qStartTime if it is null
                if(participant.QStartDate == null) { 
                    participant.QStartDate = DateTime.Now;
                }
            }
        }

        public void EndParticipantsStartTime(SessionHandler handler, string roomPin)
        {
            var participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            foreach (var participant in participants)
            {
                // Only update the participant qEndTime if it is null
                if (participant.QEndDate == null)
                {
                    participant.QEndDate = DateTime.Now;
                }
            }
        }

        public async Task SendParticipantsScoresAsync(SessionHub hub, SessionHandler handler, string roomPin)
        {
            var participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            foreach (var participant in participants)
                await hub.Clients.Group(roomPin).SendAsync("notif", $"Score: {participant.Score} | Name: {participant.QParticipantDesc}");
        }

        public async Task SaveQuizRoomDataAsync(SessionHandler handler, string hostConnectionId, int roomId, string roomPin, DateTime startTime, IEnumerable<SetQuizRoom> Setquestions)
        {
            var host = handler.GetLinkedParticipantInConnectionId(hostConnectionId);
            if (host == null) return;

            // get all participants and serialize it
            IEnumerable<QuizParticipant> participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            var rpcPayloadSaveParticipants = new Data{ Value = JsonConvert.SerializeObject(participants) };
            var rpcResponse = await _channelClient.SaveParticipantsAsync(rpcPayloadSaveParticipants);

            if (rpcResponse == null) return;
            var participantData = JsonConvert.DeserializeObject<List<QuizParticipant>>(rpcResponse.Data);

            if(participantData == null) return;

            QuizRoomData quizRoomData = new QuizRoomData();
            quizRoomData.QRoomId = roomId;
            quizRoomData.HostId = host.UserId;
            quizRoomData.SetQuizRoomJSON = JsonConvert.SerializeObject(Setquestions);
            quizRoomData.ParticipantsJSON = JsonConvert.SerializeObject(participants);
            quizRoomData.StartedDateTime = startTime;
            quizRoomData.EndedDateTime = DateTime.Now;
            quizRoomData.CreatedByUserId = host.UserId;

            var rpcPayloadSaveQuizRoomData = new Data { Value = JsonConvert.SerializeObject(quizRoomData) };
            await _channelClient.SaveRoomDataAsync(rpcPayloadSaveQuizRoomData);
        }

        public async Task<IEnumerable<QuizRoomData>> GetQuizRoomDatasAsync()
        {
            var rpcData = new Data { };
            var rpcResponse = await _channelClient.GetAllRoomDataAsync(rpcData);

            if(rpcResponse == null) return new List<QuizRoomData>();
            return JsonConvert.DeserializeObject<IEnumerable<QuizRoomData>>(rpcResponse.Data) ?? new List<QuizRoomData>();
        }
    }
}
