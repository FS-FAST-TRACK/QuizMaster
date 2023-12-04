using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;

namespace QuizMaster.API.QuizSession.Services.Grpc
{
    public class QuizRoomServices : QuizRoomService.QuizRoomServiceBase
    {
        private readonly QuizSessionDbContext _context;

        public QuizRoomServices(QuizSessionDbContext context)
        {
            _context = context;
        }
        public override async Task<RoomResponse> CreateRoom(CreateRoomRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            try 
            {
                var name = request.Room;
                var quizRoom = new QuizRoom { QRoomDesc = name };

                //Todo: Check quiz set ID

                await _context.QuizRooms.AddAsync(quizRoom);
                await _context.SaveChangesAsync();

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(quizRoom);

                return await Task.FromResult(reply);
            }
            catch (Exception ex) 
            {
                reply.Code = 500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }
        }

        public override async Task<RoomResponse> GetAllRoom(RoomsEmptyRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            try
            {
                var allRooms = _context.QuizRooms.ToArray();

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(allRooms);

                return await Task.FromResult(reply);
            }
            catch(Exception ex) 
            {
                reply.Code = 500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }

            
            
            
        }
    }
}
