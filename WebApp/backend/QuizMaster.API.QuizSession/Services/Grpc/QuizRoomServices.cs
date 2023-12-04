using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;

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
                var room = JsonConvert.DeserializeObject<CreateRoomDTO>(request.Room);

                // check if quiz set available
                int invalidId = QuizSetAvailable(room.QuestionSets);
                if(invalidId != -1)
                {
                    reply.Code = 400;
                    reply.Message = $"QuestionSet Id of {invalidId} does not exist.";
                    return await Task.FromResult(reply);
                }

                var quizRoom = new QuizRoom { QRoomDesc = room.RoomName, QRoomPin = new Random().Next(10000000,99999999) };

                QuizRoom? existingRoom = _context.QuizRooms.Where(q => q.QRoomPin == quizRoom.QRoomPin).FirstOrDefault();
                while(existingRoom != null)
                {
                    quizRoom.QRoomPin = new Random().Next(10000000, 99999999);
                    existingRoom = _context.QuizRooms.Where(q => q.QRoomPin == quizRoom.QRoomPin).FirstOrDefault();
                }

                quizRoom.RoomOptions = JsonConvert.SerializeObject(room.RoomOptions);

                var createdRoomObject = await _context.QuizRooms.AddAsync(quizRoom);
                await _context.SaveChangesAsync();

                //Todo: Check quiz set ID
                foreach (var id in room.QuestionSets)
                {
                    await _context.SetQuizRooms.AddAsync(new SetQuizRoom { QSetId = id, QRoomId = createdRoomObject.Entity.Id });
                    await _context.SaveChangesAsync();
                }


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

            
        private int QuizSetAvailable(IEnumerable<int> QuestionSetIds)
        {
            var sets = _context.QuestionSets.Where(q => q.ActiveData).Select(q=>q.SetId).ToArray();
            foreach(var id in QuestionSetIds)
            {
                if (!sets.Contains(id))
                    return id;
            }
            return -1;
        }
    }
}
