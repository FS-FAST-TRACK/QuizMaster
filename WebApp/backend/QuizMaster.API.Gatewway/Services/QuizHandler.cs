using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.Gateway.Models.Report;
using QuizMaster.API.Gateway.Services.ReportService;
using QuizMaster.API.Gateway.SystemData.Contexts;
using QuizMaster.API.Gateway.Utilities;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Diagnostics.Eventing.Reader;

namespace QuizMaster.API.Gateway.Services
{
    public class QuizHandler
    {
        private GrpcChannel _channel;
        private QuizRoomService.QuizRoomServiceClient _channelClient;
        private readonly ReportServiceHandler reportServiceHandler;
        private readonly IServiceProvider serviceProvider;
        public QuizHandler(IOptions<GrpcServerConfiguration> options, ReportServiceHandler reportServiceHandler, IServiceProvider serviceProvider)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizRoomService.QuizRoomServiceClient(_channel);
            this.reportServiceHandler = reportServiceHandler;
            this.serviceProvider = serviceProvider;
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
            handler.GenerateSessionId(roomPin); // once started, generate a session Id, will be used for report tracking
            int setIndex = 0;
            foreach (var Qset in quizSets)
            {
                // Get the Sets
                var setRequest = new SetRequest() { Id = Qset.QSetId };
                var setReply = await grpcClient.GetQuizAsync(setRequest);

                var Setquestions = JsonConvert.DeserializeObject<List<QuestionSet>>(setReply.Data);

                if (Setquestions == null) continue;

                int currentQuestion = 1;
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

                    /*
                     * Room Metadata
                     * - CurrentSetIndex
                     * - CurrentSetName
                     * - TotalNumberOfSets
                     * - CurrentQuestionIndex
                     * - CurrentQuestionName
                     * - TotalNumberOfQuestions
                     * - ParticipantsInRoom
                     */
                    await hub.Clients.Group(roomPin).SendAsync("metadata", new {
                        currentSetName = details.CurrentSetName,
                        currentSetIndex = setIndex + 1,
                        totalNumberOfSets = quizSets.Count,
                        currentQuestionIndex = currentQuestion++,
                        currentQuestionName = details.question.QStatement,
                        totalNumberOfQuestions = Setquestions.Count,
                        participantsInRoom = handler.GetParticipantLinkedConnectionsInAGroup(roomPin).Count(),
                    });

                    for (int time = timout; time >= 0; time--)
                    {
                        details.RemainingTime = time;
                        await hub.Clients.Group(roomPin).SendAsync("question", details);
                        //string connectionId = hub.Context.ConnectionId;
                        
                        //await hub.Clients.Group(roomPin).SendAsync("connId", connectionId);

                        await Task.Delay(1000);
                    }
                }
                setIndex++;
                if (room.ShowLeaderboardEachRound() && setIndex < quizSets.Count)
                {
                    await hub.Clients.Group(roomPin).SendAsync("notif", "Displaying leaderboards");
                    await SendParticipantsScoresAsync(hub, handler, roomPin, room, adminData, false); // send scores
                    await Task.Delay(10000);
                }

                // before going to new set, do some elimination if toggled
                if (room.IsEliminationRound() && setIndex < quizSets.Count)
                {
                    await EliminateParticipantsAsync(hub, handler, roomPin, adminData);
                    await hub.Clients.Client(hostConnectionId).SendAsync("notif", "Elimination, reducing population to half");
                }


            }
            //await hub.Clients.Group(roomPin).SendAsync("stop", "Quiz has ended, scores and sent");
            
            SetParticipantsEndTime(handler, roomPin); // End participant time
            var leaderboards = await SendParticipantsScoresAsync(hub, handler, roomPin, room, adminData, true); // send scores
            handler.RemoveRoomDataCurrentDisplayed(roomPin); // clear the last question
            handler.RemoveActiveRoom(room.QRoomPin); // Quiz ended, can restart | Set to inactive

            // Save the Participant's Scores and reset their data's
            await SaveQuizRoomDataAsync(handler, hostConnectionId, roomId, roomPin, startTime, quizSets, leaderboards);
            // Clear
            handler.ResetParticipantLinkedConnectionsInAGroup(roomPin);
            handler.ClearEliminatedParticipants(Convert.ToInt32(roomPin));
            //await hub.Clients.Group(roomPin).SendAsync("notif", "Quiz Data: "+JsonConvert.SerializeObject(await GetQuizRoomDatasAsync()));
            //await hub.Clients.Group(roomPin).SendAsync("stop",true);
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

        public async Task<List<LeaderboardReport>> SendParticipantsScoresAsync(SessionHub hub, SessionHandler handler, string roomPin, QuizRoom room, QuizParticipant adminData, bool isStop)
        {
            List<QuizParticipant> participants = handler.GetParticipantLinkedConnectionsInAGroup(roomPin).ToList();
            participants.AddRange(handler.GetEliminatedParticipants(Convert.ToInt32(roomPin)));
            int limitDisplayed = room.DisplayTop10Only() ? 10 : participants.Count();

            List<LeaderboardReport> leaderboardReports = new();

            var leaderboard = participants.OrderByDescending(p => p.Score).Take(limitDisplayed).Select(person => {
                if (adminData.UserId != person.UserId)
                {
                    string eliminated = handler.IsParticipantEliminated(Convert.ToInt32(roomPin), person) ? "Eliminated" : string.Empty;
                    leaderboardReports.Add(new LeaderboardReport() { ParticipantName = person.QParticipantDesc, Score = person.Score, SessionId = handler.GetSessionId(roomPin) });
                    return new ScoreDTO() { Name = person.QParticipantDesc, Score = person.Score, Eleminated = eliminated };
                }
                return null;

            } ).Where(scoreDto => scoreDto != null).ToList();
            await hub.Clients.Group(roomPin).SendAsync("leaderboard", leaderboard, isStop);

            //foreach (var participant in participants.OrderByDescending(p => p.Score).Take(limitDisplayed))
            //{
            //    i // don't display admin score
            //    if (adminData.UserId == participant.UserId) continue; // don't display admin score
            //    string eliminated = handler.IsParticipantEliminated(Convert.ToInt32(roomPin), participant) ? "Eliminated" : string.Empty;

            //    var score = new ScoreDTO (){ Name = participant.QParticipantDesc, Score = participant.Score, Eleminated= eliminated };
            //    await hub.Clients.Group(roomPin).SendAsync("leaderboard",  score);
            //}

            return leaderboardReports;
        }

        public async Task SaveQuizRoomDataAsync(SessionHandler handler, string hostConnectionId, int roomId, string roomPin, DateTime startTime, IEnumerable<SetQuizRoom> Setquestions, IEnumerable<LeaderboardReport> leaderboardReports)
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


            // Save the Session Data
            QuizRoomData quizRoomData = new QuizRoomData();
            quizRoomData.QRoomId = roomId;
            quizRoomData.SessionId = handler.GetSessionId(roomPin);
            quizRoomData.HostId = host.UserId;
            quizRoomData.SetQuizRoomJSON = JsonConvert.SerializeObject(Setquestions);
            quizRoomData.ParticipantsJSON = JsonConvert.SerializeObject(participants);
            quizRoomData.StartedDateTime = startTime;
            quizRoomData.EndedDateTime = DateTime.Now;
            quizRoomData.CreatedByUserId = host.UserId;

            var rpcPayloadSaveQuizRoomData = new Data { Value = JsonConvert.SerializeObject(quizRoomData) };
            await _channelClient.SaveRoomDataAsync(rpcPayloadSaveQuizRoomData);

            // Generate the Report Data
            QuizReport quizReport = new();
            quizReport.NoOfParticipants = participantData.Count();
            quizReport.RoomId = roomId;
            quizReport.StartTime = startTime;
            quizReport.EndTime = quizRoomData.EndedDateTime;
            quizReport.ParticipantAnswerReportsJSON = JsonConvert.SerializeObject(reportServiceHandler.GetParticipantAnswerReports(handler.GetSessionId(roomPin)));
            quizReport.LeaderboardReportsJSON = JsonConvert.SerializeObject(leaderboardReports);

            // Since we cannot instantiate scoped object in singleton, we will call the service provider and get the service
            using var scope = serviceProvider.CreateScope();
            var reportRepository = scope.ServiceProvider.GetService<ReportRepository>();

            // Save the Report Data
            reportRepository?.SaveReport(quizReport);
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
                        await handler.RemoveClientFromGroups(hub, connectionId, $"{participantLinkedConnectionId.QParticipantDesc} was eliminated", channel: "notification");
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
