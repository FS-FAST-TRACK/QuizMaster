using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Net.Http;

namespace QuizMaster.API.Gateway.Services
{
    public class SessionHandler
    {
        private IDictionary<string, string> connectionGroupPair;
        private Dictionary<string, QuizParticipant> participantLinkedConnectionId;
        private List<string> InSession;
        public SessionHandler()
        {
            connectionGroupPair = new Dictionary<string, string>();
            participantLinkedConnectionId = new Dictionary<string, QuizParticipant>();
            InSession = new List<string>();
        }

        public async Task AddToGroup(SessionHub hub, string group, string connectionId)
        {
            connectionGroupPair[connectionId] = group;
            await hub.Groups.AddToGroupAsync(connectionId, group);
        }

        public async Task RemoveFromGroup(SessionHub hub, string group, string connectionId)
        {
            await hub.Groups.RemoveFromGroupAsync(connectionId, group);
            connectionGroupPair.Remove(connectionId);
        }

        public async Task RemoveGroup(SessionHub hub, string group)
        {
            var connectionIds = connectionGroupPair.Where(kv => kv.Value.Equals(group)).Select(kv => kv.Key).ToList();
            foreach(var connectionId in connectionIds)
            {
                await hub.Groups.RemoveFromGroupAsync(connectionId, group);
                connectionGroupPair.Remove(connectionId);
            }
        }
        public async Task RemoveClientFromGroups(SessionHub hub, string connectionId, string disconnectMessage)
        {
            await hub.Groups.RemoveFromGroupAsync(connectionId, connectionGroupPair[connectionId]);
            await hub.Clients.Group(connectionGroupPair[connectionId]).SendAsync("chat", disconnectMessage);
            connectionGroupPair.Remove(connectionId);
        }

        public string? GetConnectionGroup(string connectionId)
        {
            return connectionGroupPair[connectionId];
        }

        public void LinkParticipantConnectionId(string connectionId, QuizParticipant quizParticipant)
        {
            participantLinkedConnectionId[connectionId] = quizParticipant;
        }

        public QuizParticipant GetLinkedParticipantInConnectionId(string connectionId)
        {
            return participantLinkedConnectionId[connectionId];
        }


        public IEnumerable<QuizParticipant> ParticipantLinkedConnectionsInAGroup(string group)
        {
            var connectionIds = connectionGroupPair.Where(kv => kv.Value.Equals(group)).Select(kv => kv.Key).ToList();
            List<QuizParticipant> participants = new List<QuizParticipant>();
            foreach (var connectionId in connectionIds)
            {
                participants.Add(GetLinkedParticipantInConnectionId(connectionId));
            }

            return participants;
        }

        public async Task StartQuiz(SessionHub hub, QuizRoomService.QuizRoomServiceClient grpcClient, string roomId)
        {
            if(InSession.Contains(roomId))
            {
                await hub.Clients.Caller.SendAsync("notif", "Room has already started");
                return;
            }
            InSession.Add(roomId);
            string roomPin = "";
            // Get the SetQuizRoom
            var set = new SetRequest() { Id = Convert.ToInt32(roomId) };
            var qsetReply = await grpcClient.GetQuizSetAsync(set);
            var quizSets = JsonConvert.DeserializeObject<List<SetQuizRoom>>(qsetReply.Data);

            if(quizSets == null) { await hub.Clients.Caller.SendAsync("notif", "Failed to start a room, SetQuizRoom is not available"); return; }
            if(quizSets.Count == 0)
            {
                await hub.Clients.Caller.SendAsync("notif", "Failed to start a room, There is no question sets available");
                return;
            }

            foreach (var Qset in quizSets)
            {
                // Get the Sets
                var setRequest = new SetRequest() { Id = Qset.QSetId };
                var setReply = await grpcClient.GetQuizAsync(setRequest);

                var Setquestions = JsonConvert.DeserializeObject<List<QuestionSet>>(setReply.Data);

                if (Setquestions == null) continue;
                

                foreach(var questionSet in Setquestions)
                {
                    roomPin = Qset.QRoom.QRoomPin + "";
                    await hub.Clients.Group(roomPin).SendAsync("notif", "Displaying a question");
                    var questionRequest = new SetRequest() { Id = questionSet.QuestionId };
                    var questionSetReply = await grpcClient.GetQuestionAsync(questionRequest);

                    var details = JsonConvert.DeserializeObject<QuestionsDTO>(questionSetReply.Data);
                    var timout = 5;///details.question.QTime - 1;

                    for (int time = timout; time >= 0; time--)
                    {
                        details.RemainingTime = time;
                        await hub.Clients.Group(roomPin).SendAsync("question", details);
                        await Task.Delay(1000);
                    }
                }
            }
            await hub.Clients.Group(roomPin).SendAsync("stop", "Quiz has ended");
            InSession.Remove(roomId);
        }

        public async Task<AuthStore?> GetUserInformation(SessionHub hub, AuthService.AuthServiceClient _authChannelClient, string token)
        {
            /*
             * string token = string.Empty;
            if (hub.Context == null) return null;
            if (hub.Context.User == null) return null;
            // grab the claims identity
            var tokenClaim = hub.Context.GetHttpContext().User.Claims.ToList().FirstOrDefault(e => e.Type == "token");
            if (hub.Context.GetHttpContext().Request.Cookies.TryGetValue("aaa", out var cookieValues))
            {
                // Use the cookie value as needed
            }

            // Accessing all cookies
            foreach (var cookie in hub.Context.GetHttpContext().Request.Cookies)
            {
                string cookieName = cookie.Key;
                string cookieValue = cookie.Value;
                // Process each cookie
            }
            var isAuth = hub.Context.User.Identity.IsAuthenticated;
            if (tokenClaim == null)
            {
                // Check the request header if there is a JWT token
                try
                {
                    if (hub.Context.GetHttpContext() != null)
                        token = hub.Context.GetHttpContext().Request.Headers.Authorization.ToString().Split(" ")[1];
                }
                catch
                {
                    
                }
            }
            else
            {
                token = tokenClaim.Value;
            }
             */

            if (string.IsNullOrEmpty(token)) return null;

            var request = new ValidationRequest()
            {
                Token = token
            };

            // get the AuthStore based on token
            var authStore = await _authChannelClient.ValidateAuthenticationAsync(request);

            var info = JsonConvert.DeserializeObject<AuthStore>(authStore.AuthStore);

            return info;
        }
    }
}
