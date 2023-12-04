using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using System.Threading.Channels;

namespace QuizMaster.API.Gateway.Hubs
{
    public class SessionHub : Hub
    {
        private GrpcChannel _channel;
        private  QuizRoomService.QuizRoomServiceClient _channelClient;

        public SessionHub(IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizRoomService.QuizRoomServiceClient(_channel); 
        }
        public async Task CreateRoom(string name)
        {
            var request = new CreateRoomRequest { Room = name };
            var reply = await _channelClient.CreateRoomAsync(request);

            if (reply.Code == 200)
            {
                var quizRoom = JsonConvert.DeserializeObject<QuizRoom>(reply.Data);
                await Groups.AddToGroupAsync(Context.ConnectionId, quizRoom.QRoomDesc);

                await Clients.Group(quizRoom.QRoomDesc).SendAsync("NewQuizRooms", new[] { quizRoom });
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
    }
}
