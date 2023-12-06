using Grpc.Core;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<RoomResponse> UpdateRoom(CreateRoomRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            try
            {
                var room = JsonConvert.DeserializeObject<UpdateRoomDTO>(request.Room);

                // check if quiz set available
                int invalidId = QuizSetAvailable(room.QuestionSets);
                if (invalidId != -1)
                {
                    reply.Code = 400;
                    reply.Message = $"QuestionSet Id of {invalidId} does not exist.";
                    return await Task.FromResult(reply);
                }

                var quizRoom = _context.QuizRooms.Where(q => q.Id == room.RoomId).FirstOrDefault();

                while (quizRoom == null)
                {
                    reply.Code = 404;
                    reply.Message = $"QuizRoom Id of {invalidId} does not exist.";
                    return await Task.FromResult(reply);
                }

                quizRoom.RoomOptions = JsonConvert.SerializeObject(room.RoomOptions);
                await _context.SaveChangesAsync();

                var sets =  _context.SetQuizRooms.Where(r => r.QRoomId == quizRoom.Id).ToList();
                _context.SetQuizRooms.RemoveRange(sets);
                await _context.SaveChangesAsync();
                
                foreach (var id in room.QuestionSets)
                {
                    await _context.SetQuizRooms.AddAsync(new SetQuizRoom { QSetId = id, QRoomId = quizRoom.Id });
                    await _context.SaveChangesAsync();
                }

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
                var allRooms = _context.QuizRooms.Where(a=>a.ActiveData).ToArray();

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

        public override async Task<RoomResponse> DeleteRoom(ModifyRoomRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();

            var room = _context.QuizRooms.FirstOrDefault(r => r.Id == request.Room);
            if (room == null)
            {
                reply.Code = 404;
                reply.Message = $"Room with Id {request.Room} does not exist";
                return await Task.FromResult(reply);
            }
            if (!room.ActiveData)
            {
                reply.Code = 200;
                reply.Message = $"Room with Id '{request.Room}' ({room.QRoomDesc}) was already deleted";
                return await Task.FromResult(reply);
            }
            room.ActiveData = false;
            await _context.SaveChangesAsync();

            reply.Code = 204;
            reply.Message = $"Successfully deleted '{room.QRoomDesc}'";
            reply.Data = room.QRoomPin+"";


            return await Task.FromResult(reply);
        }

        // In room, get all the Sets
        public override async Task<RoomResponse> GetQuizSet(SetRequest request, ServerCallContext context)
        {
            var repy = new RoomResponse();
            var id = request.Id;


            var quizSets = _context.SetQuizRooms.Where(x=> x.QRoomId == id).ToList();
            
            foreach( var quizSet in quizSets)
            {
                _ = await _context.QuizRooms.Where(r => r.Id == quizSet.QRoomId).FirstOrDefaultAsync();
            }

            repy.Code = 200;
            repy.Data = JsonConvert.SerializeObject(quizSets);

            return await Task.FromResult(repy);
        }

        // From all the QuestionSet
        public override async Task<RoomResponse> GetQuiz(SetRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            var id = request.Id;

            var qestions = _context.QuestionSets.Where(x=> x.SetId == id).ToList();

            reply.Code = 200;
            reply.Data = JsonConvert.SerializeObject(qestions);

            return await Task.FromResult(reply);
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

        // Get Question from a set Id in a list
        public override async Task<RoomResponse> GetQuestion(SetRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            var id = request.Id;

            var question = _context.Questions.FirstOrDefault(x => x.Id == id);
            var details = _context.QuestionDetails.Where(x => x.QuestionId == question.Id).ToList();

            if(question == null)
            {
                reply.Code = 404;
                reply.Message = $"Question with Id of {id} does not exist";

                return await Task.FromResult(reply);
            }

            reply.Code = 200;
            reply.Data = JsonConvert.SerializeObject(new QuestionsDTO { question=question, details=details});
            
            return await Task.FromResult(reply);    

        }
    }
}
