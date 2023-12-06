using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Services;
using QuizMaster.API.QuizSession.Models;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Linq;
using System.Threading.Channels;

namespace QuizMaster.API.Gateway.Hubs
{
    public class SessionHub : Hub
    {
        private GrpcChannel _channel;
        private  QuizRoomService.QuizRoomServiceClient _channelClient;
        private SessionHandler SessionHandler;

        public SessionHub(IOptions<GrpcServerConfiguration> options, SessionHandler sessionHandler)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizRoomService.QuizRoomServiceClient(_channel); 
            SessionHandler = sessionHandler;
        }

        public async Task CreateRoom(CreateRoomDTO roomDTO)
        {
            var request = new CreateRoomRequest { Room = JsonConvert.SerializeObject(roomDTO) };
            var reply = await _channelClient.CreateRoomAsync(request);

            if (reply.Code == 200)
            {
               

                var quizRoom = JsonConvert.DeserializeObject<QuizRoom>(reply.Data);
                await Groups.AddToGroupAsync(Context.ConnectionId, quizRoom.QRoomPin+"");

                await Clients.All.SendAsync("NewQuizRooms", new[] { quizRoom });
            }


            // TODO: Reply 500 status
        }


        public async Task DeleteRoom(int roomId)
        {
            var request = new ModifyRoomRequest { Room = roomId };

            var reply = await _channelClient.DeleteRoomAsync(request);

            if (reply.Code == 204)
            {
                await Clients.Caller.SendAsync("chat", "Room was deleted");
                await Clients.Group(reply.Data).SendAsync("chat", "[System] You have been removed from the room");
                await SessionHandler.RemoveGroup(this, reply.Data);
            }
            else
            {
                await Clients.Caller.SendAsync("notif", reply.Message);
            }
        }

        public async Task UpdateRoom(UpdateRoomDTO updateRoomDTO)
        {
            var request = new CreateRoomRequest { Room = JsonConvert.SerializeObject(updateRoomDTO) };
            var reply = await _channelClient.UpdateRoomAsync(request);

            // TODO:
            if (reply.Code == 200)
            {
                var quizRoom = JsonConvert.DeserializeObject<QuizRoom>(reply.Data);
                await Clients.Caller.SendAsync("NewQuizRooms", new[] { quizRoom });
                await Clients.Caller.SendAsync("chat", "Room was updated");
            }
            else
            {
                await Clients.Caller.SendAsync("chat", reply.Message);
            }
        }


        public async Task Chat(string chat, string roomId)
        {
            string connectionId = Context.ConnectionId;

            // send chat only to group
            await Clients.Group(roomId).SendAsync("chat", $"[{connectionId}]: {chat}");
        }


        public async Task JoinRoom(int RoomPin)
        {
            string connectionId = Context.ConnectionId;
            try
            {
                var reply = _channelClient.GetAllRoom(new RoomsEmptyRequest());

                if (reply.Code == 200)
                {
                    var quizRooms = JsonConvert.DeserializeObject<QuizRoom[]>(reply.Data);

                    bool containsId = false;
                    var room = new QuizRoom();
                    foreach(QuizRoom rooms in quizRooms)
                    {
                        if(rooms.QRoomPin == RoomPin)
                        {
                            room = rooms;
                            containsId = true;
                            break;
                        }
                    }

                    if (containsId)
                    {
                        await Groups.AddToGroupAsync(connectionId, $"{RoomPin}");
                        await SessionHandler.AddToGroup(this, $"{RoomPin}", connectionId);
                        await Clients.Group($"{RoomPin}").SendAsync("notif",$"{connectionId} has joined Room {room.QRoomDesc}", room );
                    }
                    //await Clients.All.SendAsync("QuizRooms", quizRooms);
                }
        }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        public async Task GetAllRooms() 
        {
            try
            {
                var reply = _channelClient.GetAllRoom(new RoomsEmptyRequest());

                if (reply.Code == 200)
                {
                    var quizRooms = JsonConvert.DeserializeObject<QuizRoom[]>(reply.Data);

                    await Clients.All.SendAsync("QuizRooms", quizRooms);
                }
            }
            catch (Exception ex) 
            {
                Console.Write(ex.ToString());
            }
            
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await SessionHandler.RemoveClientFromGroups(this, Context.ConnectionId, $"{Context.ConnectionId} has been disconnected");
        }

        public async Task LeaveRoom()
        {
            await SessionHandler.RemoveClientFromGroups(this, Context.ConnectionId, $"{Context.ConnectionId} has left the room");
        }
        public async Task StartRoom()
        {
            try
            {
                var roomPin = SessionHandler.GetConnectionGroup(Context.ConnectionId);
                if (roomPin != null)
                {
                    await Clients.Group(roomPin).SendAsync("start", true);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex?.ToString());
            }
        }

        public async Task GetQuestionSets(int id)
        {
            var set = new SetRequest() { Id = id };
            var setReply = await _channelClient.GetQuizSetAsync(set);

            if(setReply.Code == 200)
            {
                var quizSets = JsonConvert.DeserializeObject<List<SetQuizRoom>>(setReply.Data);

                var roomPin = SessionHandler.GetConnectionGroup(Context.ConnectionId);
                if (roomPin != null)
                {
                    await Clients.Group(roomPin).SendAsync("questionSet", quizSets);
                }
            }
        }

        public async Task GetQuestions(int id)
        {
            var request = new SetRequest() { Id = id };
            var reply = await _channelClient.GetQuizAsync(request);

            if (reply.Code == 200)
            {
                var questions = JsonConvert.DeserializeObject<List<QuestionSet>>(reply.Data);
                var roomPin = SessionHandler.GetConnectionGroup(Context.ConnectionId);
                if (roomPin != null)
                {
                    foreach(var question in questions)
                    {
                        var questionRequest = new SetRequest() { Id = question.QuestionId };
                        var questionReply = _channelClient.GetQuestion(questionRequest);

                        if(questionReply.Code == 200) 
                        {
                            var questionDetails = JsonConvert.DeserializeObject<Question>(questionReply.Data);
                            var timout = questionDetails.QTime;

                            await Clients.Group(roomPin).SendAsync("question", questionDetails.QStatement);
                            await Task.Delay(timout*1000);
                        }
                    }
                }
            }
        }

    }
}
