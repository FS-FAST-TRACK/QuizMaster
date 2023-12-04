using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.QuizSession.Models;
using QuizMaster.API.QuizSession.Protos;
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
        private IDictionary<string, string> ConnectionIdAndGroupPair = new Dictionary<string, string>();

        public SessionHub(IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizRoomService.QuizRoomServiceClient(_channel); 
        }

        public async Task CreateRoom(CreateRoomDTO roomDTO)
        {
            var request = new CreateRoomRequest { Room = JsonConvert.SerializeObject(roomDTO) };
            var reply = await _channelClient.CreateRoomAsync(request);

            if (reply.Code == 200)
            {
                var quizRoom = JsonConvert.DeserializeObject<QuizRoom>(reply.Data);
                await Groups.AddToGroupAsync(Context.ConnectionId, quizRoom.QRoomDesc);

                await Clients.Group(quizRoom.QRoomDesc).SendAsync("NewQuizRooms", new[] { quizRoom });
            }
            // TODO: Reply 500 status
        }


        public async Task Chat(string chat, string roomId)
        {
            string connectionId = Context.ConnectionId;

            // send chat only to group
            await Clients.Group(roomId).SendAsync("notif", $"[{connectionId}]: {chat}");
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
                    string roomName = string.Empty;
                    foreach(QuizRoom room in quizRooms)
                    {
                        if(room.QRoomPin == RoomPin)
                        {
                            roomName = room.QRoomDesc;
                            containsId = true;
                            break;
                        }
                    }

                    if (containsId)
                    {
                        await Groups.AddToGroupAsync(connectionId, $"{RoomPin}");
                        ConnectionIdAndGroupPair.Add(connectionId, roomName);
                        await Clients.Group($"{RoomPin}").SendAsync("notif",$"{connectionId} has joined Room {roomName}");
                    }
                    //await Clients.All.SendAsync("QuizRooms", quizRooms);
                }
        }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        /* task harold
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var clientConnectionId = Context.ConnectionId;

            await Groups.RemoveFromGroupAsync(clientConnectionId, ConnectionIdAndGroupPair[clientConnectionId]);

            // retrieve the clientConnectionId from participants list then send it to notif
            // must check if what group does the user belong and send a notif there
            await Clients.Group(ConnectionIdAndGroupPair[clientConnectionId]).SendAsync("notif", $"{clientConnectionId} has left");
        }*/

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
    }
}
