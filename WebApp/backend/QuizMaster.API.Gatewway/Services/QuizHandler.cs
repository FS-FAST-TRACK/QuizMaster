using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.Gateway.Utilities;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;

namespace QuizMaster.API.Gateway.Services
{
    public class QuizHandler
    {
        private GrpcChannel _channel;
        private QuizRoomService.QuizRoomServiceClient _channelClient;
        public QuizHandler(IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizRoomService.QuizRoomServiceClient(_channel);
        }
        public async Task StartQuiz(SessionHub hub, SessionHandler handler, QuizRoomService.QuizRoomServiceClient grpcClient, QuizRoom room)
        {
            int roomId = room.Id;
            string hostConnectionId = hub.Context.ConnectionId;
            var adminData = handler.GetLinkedParticipantInConnectionId(hostConnectionId);
            DateTime startTime = DateTime.UtcNow;
            // Check if the current session is started, otherwise don't proceed
            if (handler.IsRoomActive(room.QRoomPin))
            {
                await hub.Clients.Caller.SendAsync("notif", "Room has already started");
                return;
            }

            if(adminData == null)
            {
                await hub.Clients.Caller.SendAsync("notif", "Could not start, admin data is not valid");
                return;
            }

            string roomPin = "";
            // Get the SetQuizRoom
            var set = new SetRequest() { Id = roomId };
            var qsetReply = await grpcClient.GetQuizSetAsync(set);
            var quizSets = JsonConvert.DeserializeObject<List<SetQuizRoom>>(qsetReply.Data);

            if (quizSets == null) { await hub.Clients.Caller.SendAsync("notif", "Failed to start a room, SetQuizRoom is not available"); return; }
            if (quizSets.Count == 0)
            {
                await hub.Clients.Caller.SendAsync("notif", "Failed to start a room, There is no question sets available");
                return;
            }

            handler.AddActiveRoom(room.QRoomPin, room); // register the room to be started | set to active
            int setIndex = 0;
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
                    var timout = details.question.QTime;

                    // setting the current set info
                    details.CurrentSetName = questionSet.Set.QSetName;
                    details.CurrentSetDesc = questionSet.Set.QSetDesc;
                    details.details.ForEach(qD => qD.DetailTypes = new List<DetailType>());

                    for (int time = timout; time >= 0; time--)
                    {
                        details.RemainingTime = time;
                        await hub.Clients.Group(roomPin).SendAsync("question", details);
                        await Task.Delay(1000);
                    }
                }

                // before going to new set, do some elimination if toggled
                if (room.IsEliminationRound() && ++setIndex < quizSets.Count)
                {
                    if (room.ShowLeaderboardEachRound())
                    {
                        await hub.Clients.Client(hostConnectionId).SendAsync("notif", "Displaying leaderboards");
                        await SendParticipantsScoresAsync(hub, handler, roomPin, room, adminData); // send scores
                        await Task.Delay(5000);
                    }
                    await EliminateParticipantsAsync(hub, handler, roomPin, adminData);
                    await hub.Clients.Client(hostConnectionId).SendAsync("notif", "Elimination, reducing population to half");
                }
            }
            await hub.Clients.Group(roomPin).SendAsync("stop", "Quiz has ended, scores and sent");
            
            SetParticipantsEndTime(handler, roomPin); // End participant time
            await SendParticipantsScoresAsync(hub, handler, roomPin, room, adminData); // send scores
            handler.RemoveRoomDataCurrentDisplayed(roomPin); // clear the last question
            handler.RemoveActiveRoom(room.QRoomPin); // Quiz ended, can restart | Set to inactive

            // Save the Participant's Scores and reset their data's
            await SaveQuizRoomDataAsync(handler, hostConnectionId, roomId, roomPin, startTime, quizSets);
            // Clear
            handler.ResetParticipantLinkedConnectionsInAGroup(roomPin);
            handler.ClearEliminatedParticipants(Convert.ToInt32(roomPin));
            //await hub.Clients.Group(roomPin).SendAsync("notif", "Quiz Data: "+JsonConvert.SerializeObject(await GetQuizRoomDatasAsync()));
            await hub.Clients.Group(roomPin).SendAsync("notif", "[THE QUIZ HAS ENDED]");
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

        public void SetParticipantsEndTime(SessionHandler handler, string roomPin)
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

        public async Task SendParticipantsScoresAsync(SessionHub hub, SessionHandler handler, string roomPin, QuizRoom room, QuizParticipant adminData)
        {
            List<QuizParticipant> participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin).ToList();
            participants.AddRange(handler.GetEliminatedParticipants(Convert.ToInt32(roomPin)));
            int limitDisplayed = room.DisplayTop10Only() ? 10 : participants.Count();
            foreach (var participant in participants.OrderByDescending(p => p.Score).Take(limitDisplayed))
            {
                if (adminData.UserId == participant.UserId) continue; // don't display admin score
                string eliminated = handler.IsParticipantEliminated(Convert.ToInt32(roomPin), participant) ? "Eliminated" : string.Empty;
                await hub.Clients.Group(roomPin).SendAsync("leaderboard", $"Score: {participant.Score} | Name: {participant.QParticipantDesc} {eliminated}");
            }


        }

        public async Task SaveQuizRoomDataAsync(SessionHandler handler, string hostConnectionId, int roomId, string roomPin, DateTime startTime, IEnumerable<SetQuizRoom> Setquestions)
        {
            var host = handler.GetLinkedParticipantInConnectionId(hostConnectionId);
            if (host == null) return;

            // get all participants and serialize it
            IEnumerable<QuizParticipant> participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            participants.ToList().AddRange(handler.GetEliminatedParticipants(roomId)); // add the eliminated participants
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

        public async Task EliminateParticipantsAsync(SessionHub hub, SessionHandler handler, string roomPin, QuizParticipant hostData)
        {
            var participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin);
            int population = participants.Count() - 1; // exclude 1 for the host

            // we don't need to kick the last participant if he/she is left in a group
            if (population == 1) return;

            int half = population / 2;
            /*
             * Sorts the participant list by scores starting from the lowest,
             * now we will eliminate all participants with low scores and retain
             * the high score participants. 50% of the population will be reduced.
             * The lowest score participant is the highest chance to be removed
             */
            foreach(var participant in participants.Where(p => p.UserId != hostData.UserId).OrderBy(x => x.Score).ToList().Take(half))
            {
                foreach(var connectionId in handler.GetConnectionIdsInAGroup(roomPin)){
                    var participantLinkedConnectionId = handler.GetLinkedParticipantInConnectionId(connectionId);
                    if (participantLinkedConnectionId == null) continue;
                    if (participantLinkedConnectionId.Id == participant.Id && participantLinkedConnectionId.UserId == participant.UserId && participantLinkedConnectionId.QParticipantDesc == participant.QParticipantDesc)
                    {
                        participantLinkedConnectionId.QEndDate = DateTime.Now;
                        // remove the connectionId from the group
                        await handler.RemoveClientFromGroups(hub, connectionId, $"{participantLinkedConnectionId.QParticipantDesc} was eliminated");
                        await hub.Clients.Client(connectionId).SendAsync("notif", "You are eliminated");
                        //await hub.Clients.Group(roomPin).SendAsync("notif", $"{participantLinkedConnectionId.QParticipantDesc} was eliminated");
                        // add to eliminated participant
                        handler.AddEliminatedParticipant(Convert.ToInt32(roomPin), participantLinkedConnectionId);
                    }
                };
            };
        }
    }
}
