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
        private Dictionary<string, string> connectionGroupPair;
        private Dictionary<string, QuizParticipant> participantLinkedConnectionId;
        private List<QuizParticipant> AbandonedParticipants;
        private List<string> InSession;
        private Dictionary<string, string> AuthenticatedClientIds;
        private List<string> AuthenticatedClientAdmins;
        public SessionHandler()
        {
            connectionGroupPair = new Dictionary<string, string>();
            participantLinkedConnectionId = new Dictionary<string, QuizParticipant>();
            InSession = new List<string>();
            AuthenticatedClientIds= new Dictionary<string, string>();
            AuthenticatedClientAdmins = new List<string>();
            AbandonedParticipants = new();
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
            var hasAbandonedParticipant = participantLinkedConnectionId.Where(kv => kv.Value.UserId == quizParticipant.UserId && kv.Value.QEndDate != null).Select(kv=>kv.Value).FirstOrDefault();

            // Link back the disconnected connectionId to a participant that hasn't completed the session yet
            if(hasAbandonedParticipant != null)
                participantLinkedConnectionId.Add(connectionId, hasAbandonedParticipant);
            else 
                participantLinkedConnectionId.Add(connectionId, quizParticipant);
        }

        public QuizParticipant? GetLinkedParticipantInConnectionId(string connectionId)
        {
            return participantLinkedConnectionId.GetValueOrDefault(connectionId);
        }


        public IEnumerable<QuizParticipant> ParticipantLinkedConnectionsInAGroup(string group)
        {
            var connectionIds = connectionGroupPair.Where(kv => kv.Value.Equals(group)).Select(kv => kv.Key).ToList();
            List<QuizParticipant> participants = new List<QuizParticipant>();
            foreach (var connectionId in connectionIds)
            {
                var participant = GetLinkedParticipantInConnectionId(connectionId);
                if(participant != null) { participants.Add(participant); }
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

                    if(details == null) continue;

                    var timout = details.question.QTime;

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

        public async Task AuthenticateConnectionId(SessionHub hub, AuthService.AuthServiceClient _authChannelClient, string connectionId, string token)
        {
            if (string.IsNullOrEmpty(token)) return;

            var request = new ValidationRequest()
            {
                Token = token
            };

            // get the AuthStore based on token
            var authStore = await _authChannelClient.ValidateAuthenticationAsync(request);

            var info = JsonConvert.DeserializeObject<AuthStore>(authStore.AuthStore);

            // check if auth store is not null
            if (info == null)
            {
                await hub.Clients.Caller.SendAsync("notif", "Failed to authenticate");
                return;
            }


            AuthenticatedClientIds[connectionId] = token;
            if (info.Roles.Contains("Administrator"))
            {
                AuthenticatedClientAdmins.Add(connectionId);
            }

            await hub.Clients.Caller.SendAsync("notif", "Logged in successfully");
        }

        public bool IsAuthenticated(string connectionId)
        {
            return AuthenticatedClientIds.ContainsKey(connectionId);
        }

        public bool IsAdmin(string connectionId)
        {
            return AuthenticatedClientAdmins.Contains(connectionId);
        }

        public string? GetTokenConnectionOwner(string token)
        {
            return AuthenticatedClientIds.Where(kv => kv.Value == token).Select(k => k.Key).FirstOrDefault();
        }

        public string GetConnectionToken(string connectionId)
        {
            return AuthenticatedClientIds[connectionId];
        }

        public void UnbindConnectionId(string connectionId)
        {
            AuthenticatedClientAdmins.Remove(connectionId);
            AuthenticatedClientIds.Remove(connectionId);
            connectionGroupPair.Remove(connectionId);

            var participant = GetLinkedParticipantInConnectionId(connectionId);
            if(participant != null) AbandonedParticipants.Add(participant);
            participantLinkedConnectionId.Remove(connectionId);
        }

        public async Task<AuthStore?> GetUserInformation(AuthService.AuthServiceClient _authChannelClient, string token)
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
