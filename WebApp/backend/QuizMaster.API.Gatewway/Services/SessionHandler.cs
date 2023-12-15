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
        
        private Dictionary<string, string> AuthenticatedClientIds;
        private List<string> AuthenticatedClientAdmins;
        private Dictionary<string, int> RoomCurrentQuestionDisplayed;
        private List<string> ClientsSubmittedAnswers;
        private Dictionary<int, QuizRoom> ActiveRooms;
        private Dictionary<int, IEnumerable<QuizParticipant>> RoomEliminatedParticipants;
        public SessionHandler()
        {
            connectionGroupPair = new();
            participantLinkedConnectionId = new();
            AuthenticatedClientIds= new();
            AuthenticatedClientAdmins = new();
            AbandonedParticipants = new();
            RoomCurrentQuestionDisplayed = new();
            ClientsSubmittedAnswers = new();
            ActiveRooms = new();
            RoomEliminatedParticipants = new();
        }

        public async Task AddToGroup(SessionHub hub, string group, string connectionId)
        {
            connectionGroupPair[connectionId] = group;
            await hub.Groups.AddToGroupAsync(connectionId, group);

            IEnumerable<object> participants = GetParticipantLinkedConnectionsInAGroup(connectionGroupPair[connectionId]).Select(p => new { p.UserId, p.QParticipantDesc });
            await hub.Clients.Group(connectionGroupPair[connectionId]).SendAsync("participants", participants);
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
        public async Task RemoveClientFromGroups(SessionHub hub, string connectionId, string disconnectMessage, string channel = "chat")
        {
            if (connectionGroupPair.ContainsKey(connectionId))
            {
                await hub.Groups.RemoveFromGroupAsync(connectionId, connectionGroupPair[connectionId]);
                await hub.Clients.Group(connectionGroupPair[connectionId]).SendAsync(channel, disconnectMessage);

                IEnumerable<object> participants = GetParticipantLinkedConnectionsInAGroup(connectionGroupPair[connectionId]).Select(p => new { p.UserId, p.QParticipantDesc });
                await hub.Clients.Group(connectionGroupPair[connectionId]).SendAsync("participants", participants);

                connectionGroupPair.Remove(connectionId);
                
            }
        }

        public string? GetConnectionGroup(string connectionId)
        {
            return connectionGroupPair.GetValueOrDefault(connectionId);
        }

        public void HoldClientAnswerSubmission(string connectionId)
        {
            ClientsSubmittedAnswers.Add(connectionId);
        }

        public bool ClientOnHoldAnswerSubmission(string connectionId)
        {
            return ClientsSubmittedAnswers.Contains(connectionId);
        }

        public bool RemoveHoldOnAnswerSubmission(string connectionId)
        {
            return ClientsSubmittedAnswers.Remove(connectionId);
        }

        public void AddActiveRoom(int roomPin, QuizRoom room)
        {
            ActiveRooms.Add(roomPin, room);
        }

        public QuizRoom GetActiveRoom(int roomPin)
        {
            return ActiveRooms[roomPin];
        }

        public void RemoveActiveRoom(int roomPin)
        {
            ActiveRooms.Remove(roomPin);
        }

        public bool IsRoomActive(int roomPin)
        {
            return ActiveRooms.ContainsKey(roomPin);
        }

        public void AddEliminatedParticipant(int roomPin, QuizParticipant participant)
        {
            if(!RoomEliminatedParticipants.ContainsKey(roomPin)) 
            {
                RoomEliminatedParticipants.Add(roomPin, new List<QuizParticipant>() { participant });
            }
            else
            {
                RoomEliminatedParticipants[roomPin].ToList().Add(participant);
            }
        }

        public bool IsParticipantEliminated(int roomPin, QuizParticipant participant)
        {
            if (!RoomEliminatedParticipants.ContainsKey(roomPin))
            {
                return false;
            }
            return RoomEliminatedParticipants[roomPin].Where(p => p.QRoomId == participant.QRoomId && p.UserId == participant.UserId && p.QParticipantDesc == participant.QParticipantDesc).Any();
        }

        public void ClearEliminatedParticipants(int roomPin)
        {
            RoomEliminatedParticipants[roomPin] = new List<QuizParticipant>();
        }

        public IEnumerable<QuizParticipant> GetEliminatedParticipants(int roomPin)
        {
            return RoomEliminatedParticipants.GetValueOrDefault(roomPin) ?? new List<QuizParticipant>();
        }

        public void LinkParticipantConnectionId(string connectionId, QuizParticipant quizParticipant)
        {
            var hasAbandonedParticipant = participantLinkedConnectionId.Where(kv => kv.Value.UserId == quizParticipant.UserId && kv.Value.QEndDate != null).Select(kv=>kv.Value).FirstOrDefault();

            // Link back the disconnected connectionId to a participant that hasn't completed the session yet
            if(hasAbandonedParticipant != null)
            {
                if (!participantLinkedConnectionId.ContainsKey(connectionId))
                    participantLinkedConnectionId.Add(connectionId, hasAbandonedParticipant);
                else participantLinkedConnectionId[connectionId] = hasAbandonedParticipant;
            }
            else if(!participantLinkedConnectionId.ContainsKey(connectionId))
                participantLinkedConnectionId.Add(connectionId, quizParticipant);
        }

        public QuizParticipant? GetLinkedParticipantInConnectionId(string connectionId)
        {
            return participantLinkedConnectionId.GetValueOrDefault(connectionId);
        }

        public bool RemoveLinkedParticipantInConnectionId(string connectionId)
        {
            return participantLinkedConnectionId.Remove(connectionId);
        }

        public IEnumerable<string> GetConnectionIdsInAGroup(string group)
        {
            var connectionIds = connectionGroupPair.Where(kv => kv.Value.Equals(group)).Select(kv => kv.Key).ToList();
            return connectionIds ?? new List<string>();
        }


        public IEnumerable<QuizParticipant> GetParticipantLinkedConnectionsInAGroup(string group)
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

        public void ResetParticipantLinkedConnectionsInAGroup(string group)
        {
            var connectionIds = connectionGroupPair.Where(kv => kv.Value.Equals(group)).Select(kv => kv.Key).ToList();
            foreach (var connectionId in connectionIds)
            {
                var participant = GetLinkedParticipantInConnectionId(connectionId);
                if (participant != null) {
                    var p = new QuizParticipant { UserId =  participant.UserId, QParticipantDesc = participant.QParticipantDesc };
                    RemoveLinkedParticipantInConnectionId(connectionId);
                    LinkParticipantConnectionId(connectionId, p);
                }
            }

        }

        public void SetCurrentQuestionDisplayed(string roomPin, int questinId)
        {
            RoomCurrentQuestionDisplayed[roomPin] = questinId;
        }

        public void RemoveRoomDataCurrentDisplayed(string roomPin)
        {
            RoomCurrentQuestionDisplayed.Remove(roomPin);
        }

        public async Task<string> SubmitAnswer(QuizRoomService.QuizRoomServiceClient grpcClient, string connectionId, int questionId, string answer)
        {
            /*
             * If the current displayed questionId is not the same as passed questionId
             * deny the request and do not update the participant score
             */

            // retrieve the current group of the user
            string? userGroup = GetConnectionGroup(connectionId);
            if (userGroup == null) return "Not in session";
            if (ClientOnHoldAnswerSubmission(connectionId)) return "Already Submitted";

            if (!RoomCurrentQuestionDisplayed.ContainsKey(userGroup)) return "Not in session";

            int? currentQuestionIdDisplayed = RoomCurrentQuestionDisplayed.GetValueOrDefault(userGroup);
            if (currentQuestionIdDisplayed == null) return "Not in session";
            if (questionId != currentQuestionIdDisplayed)
            {
                //await hub.Clients.Client(connectionId).SendAsync("notif", "Question expired, your current answer is declined");
                return "Question Expired, your answer is declined";
            }
            //await hub.Clients.Client(connectionId).SendAsync("notif", "Answer Submitted");
            // get the question data
            #region GettingQuestionData
            var gRpcRequest = new SetRequest() { Id = questionId };
            var gRpcReply = await grpcClient.GetQuestionAsync(gRpcRequest);
            if (gRpcReply == null) return "Failed to retrieve question information";

            // parse the data
            var questionData = JsonConvert.DeserializeObject<QuestionsDTO>(gRpcReply.Data); 
            if (questionData == null) return "Failed to retrieve question information";

            // check if user has the correct answer
            var answers = questionData.details.Where(a => a.DetailTypes.Where(dt => dt.DTypeDesc.ToLower() == "answer").Select(Dt => Dt.DTypeDesc).ToList().Count > 0).Select(d => d.QDetailDesc).ToList();

            bool correct = false;
            foreach(string Qanswer in answers)
            {
                if(Qanswer.ToLower() == answer.ToLower())
                {
                    correct = true;
                    break;
                }
            }

            if (correct)
            {
                // get the participant data
                var participantData = GetLinkedParticipantInConnectionId(connectionId);
                if (participantData == null) return "Participant data not found";

                // increment score by 1
                participantData.Score += 1;

                
            }
            // Hold Submission of Answer
            HoldClientAnswerSubmission(connectionId);
            #endregion
            return "Answer submitted";
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
