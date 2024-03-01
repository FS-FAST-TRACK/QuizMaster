using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Helpers;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Collections.Immutable;
using static QuizMaster.API.Monitoring.Proto.RoomAuditService;

namespace QuizMaster.API.QuizSession.Services.Grpc
{
    public class QuizRoomServices : QuizRoomService.QuizRoomServiceBase
    {
        private readonly QuizSessionDbContext _context;
        private readonly RoomAuditServiceClient _roomAuditServiceClient;

        public QuizRoomServices(QuizSessionDbContext context, RoomAuditServiceClient roomAuditServiceClient)
        {
            _context = context;
            _roomAuditServiceClient = roomAuditServiceClient;
        }
        public override async Task<RoomResponse> CreateRoom(CreateRoomRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            try
            {
                var room = JsonConvert.DeserializeObject<CreateRoomDTO>(request.Room);

                // check if quiz set available
                int invalidId = QuizSetAvailable(room.QuestionSets);
                if (invalidId == -1)
                {
                    reply.Code = 400;
                    reply.Message = $"QuestionSet Id of {invalidId} does not exist.";
                    return await Task.FromResult(reply);
                }

                // Capture the details of the user creating the room
                var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                var quizRoom = new QuizRoom { QRoomDesc = room.RoomName, QRoomPin = new Random().Next(10000000, 99999999) };

                QuizRoom? existingRoom = _context.QuizRooms.Where(q => q.QRoomPin == quizRoom.QRoomPin).FirstOrDefault();
                while (existingRoom != null)
                {
                    quizRoom.QRoomPin = new Random().Next(10000000, 99999999);
                    existingRoom = _context.QuizRooms.Where(q => q.QRoomPin == quizRoom.QRoomPin).FirstOrDefault();
                }

                quizRoom.RoomOptions = JsonConvert.SerializeObject(room.RoomOptions);

                var createdRoomObject = await _context.QuizRooms.AddAsync(quizRoom);
                await _context.SaveChangesAsync();

                // Todo: Check quiz set ID
                foreach (var id in room.QuestionSets)
                {
                    await _context.SetQuizRooms.AddAsync(new SetQuizRoom { QSetId = id, QRoomId = createdRoomObject.Entity.Id });
                    await _context.SaveChangesAsync();
                }

                // Construct the create room event
                var createRoomEvent = new CreateRoomEvent
                {
                    UserId = int.Parse(userId!),
                    Username = userNameClaim,
                    Action = "Create Room",
                    Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                    Details = $"Room created by: {userNameClaim}",
                    Userrole = userRoles,
                    OldValues = "",
                    NewValues = JsonConvert.SerializeObject(quizRoom),
                };

                var logRequest = new LogCreateRoomEventRequest
                {
                    Event = createRoomEvent
                };

                try
                {
                    // Make the gRPC call to log the create room event
                    _roomAuditServiceClient.LogCreateRoomEvent(logRequest);
                }
                catch (Exception ex)
                {
                    reply.Code = 500;
                    reply.Message = ex.Message;
                    return await Task.FromResult(reply);
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

        public override async Task<RoomResponse> GetSetQuizRoom(SetRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            try
            {
                var roomId = request.Id;
                var setQuizRoom = await _context.SetQuizRooms.Where(qR => qR.QRoomId == roomId).ToListAsync();
                
               

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(setQuizRoom);

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
                if (invalidId == -1)
                {
                    reply.Code = 400;
                    reply.Message = $"QuestionSet Id of {invalidId} does not exist.";
                    return await Task.FromResult(reply);
                }

                var quizRoom = _context.QuizRooms.AsNoTracking().FirstOrDefault(q => q.Id == room.RoomId);

                while (quizRoom == null)
                {
                    reply.Code = 404;
                    reply.Message = $"QuizRoom Id of {invalidId} does not exist.";
                    return await Task.FromResult(reply);
                }

                // Capture the details of the user updating the room
                var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                var oldValues = JsonConvert.SerializeObject(quizRoom);

                quizRoom = _context.QuizRooms.FirstOrDefault(q => q.Id == room.RoomId);
                quizRoom.RoomOptions = JsonConvert.SerializeObject(room.RoomOptions);
                await _context.SaveChangesAsync();

                var sets = _context.SetQuizRooms.Where(r => r.QRoomId == quizRoom.Id).ToList();
                _context.SetQuizRooms.RemoveRange(sets);
                await _context.SaveChangesAsync();

                // Todo: Check quiz set ID
                foreach (var id in room.QuestionSets)
                {
                    await _context.SetQuizRooms.AddAsync(new SetQuizRoom { QSetId = id, QRoomId = quizRoom.Id });
                    await _context.SaveChangesAsync();
                }

                // Construct the update room event
                var updateRoomEvent = new UpdateRoomEvent
                {
                    UserId = int.Parse(userId!),
                    Username = userNameClaim,
                    Action = "Update Room",
                    Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                    Details = $"Room updated by: {userNameClaim}",
                    Userrole = userRoles,
                    OldValues = oldValues,
                    NewValues = JsonConvert.SerializeObject(quizRoom),
                };

                var logRequest = new LogUpdateRoomEventRequest
                {
                    Event = updateRoomEvent
                };

                try
                {
                    // Make the gRPC call to log the update room event
                    _roomAuditServiceClient.LogUpdateRoomEvent(logRequest);
                }
                catch (Exception ex)
                {
                    reply.Code = 500;
                    reply.Message = ex.Message;
                    return await Task.FromResult(reply);
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

        public override async Task<RoomResponse> DeleteRoom(ModifyRoomRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();

            var room = _context.QuizRooms.AsNoTracking().FirstOrDefault(r => r.Id == request.Room);
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

            // Capture the details of the user deleting the room
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // Construct the delete room event
            var deleteRoomEvent = new DeleteRoomEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Delete Room",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Room deleted by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(room),
                NewValues = "",
            };

            var logRequest = new LogDeleteRoomEventRequest
            {
                Event = deleteRoomEvent
            };

            try
            {
                // Make the gRPC call to log the delete room event
                _roomAuditServiceClient.LogDeleteRoomEvent(logRequest);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Message = ex.Message;
                return await Task.FromResult(reply);
            }

            room.ActiveData = false;
            await _context.SaveChangesAsync();

            reply.Code = 204;
            reply.Message = $"Successfully deleted '{room.QRoomDesc}'";
            reply.Data = room.QRoomPin + "";

            return await Task.FromResult(reply);
        }

        public override async Task<RoomResponse> DeactivateRoomRequest(DeactivateRoom request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            try
            {
                var room = await _context.QuizRooms.Where(r => r.Id == request.Id).FirstOrDefaultAsync();
                if (room == null)
                {
                    reply.Code = 404;
                    reply.Message = $"Room with room pin {request.Id} does not exists";
                }
                else
                {
                    room.ActiveData = false;
                    _context.SaveChanges();

                    reply.Code = 200;
                }
            }
            catch (Exception ex)
            {
                reply.Code=500;
                reply.Message=ex.Message;
            }
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

            var qestions = _context.QuestionSets.Where(x=> x.SetId == id).Include(s => s.Set).ToList();

            reply.Code = 200;
            reply.Data = JsonConvert.SerializeObject(qestions);

            return await Task.FromResult(reply);
        }

        private int QuizSetAvailable(IEnumerable<int> QuestionSetIds)
        {
            var sets = _context.QuestionSets.Where(q => q.ActiveData).Select(q=>q.SetId).ToArray();
            int foundId = -1;
            foreach(var id in QuestionSetIds)
            {
                if(sets.Contains(id))
                {
                    foundId = id; break;
                };
            }
            return foundId ;
        }

        // Get Question from a set Id in a list
        public override async Task<RoomResponse> GetQuestion(SetRequest request, ServerCallContext context)
        {
            var reply = new RoomResponse();
            var id = request.Id;

            var question = _context.Questions.FirstOrDefault(x => x.Id == id);
            //_ = _context.DetailTypes.ToList();
            //var details = _context.QuestionDetails.Where(x => x.QuestionId == question.Id).Include(qD => qD.DetailTypes).ToList();
            var details = _context.QuestionDetails.Where(x => x.QuestionId == question.Id).Include(qD => qD.DetailTypes).ToList();
            details.ToList().ForEach(qDetail =>
            {
                qDetail.DetailTypes = _context.QuestionDetailTypes.Where(qDetailType => qDetailType.QuestionDetailId == qDetail.Id).Select((qDetailType) => qDetailType.DetailType).ToList();
            });
            if (question == null)
            {
                reply.Code = 404;
                reply.Message = $"Question with Id of {id} does not exist";

                return await Task.FromResult(reply);
            }

            reply.Code = 200;
            reply.Data = JsonConvert.SerializeObject(new QuestionsDTO { question=question, details=details});
            
            return await Task.FromResult(reply);    

        }

        public override Task<RoomResponse> GetRoomData(Data request, ServerCallContext context)
        {
            return base.GetRoomData(request, context);
        }

        public override async Task<RoomResponse> SaveRoomData(Data request, ServerCallContext context)
        {
            var response = new RoomResponse();
            var quizRoomData = JsonConvert.DeserializeObject<QuizRoomData>(request.Value);

            if(quizRoomData == null) 
            {
                response.Code = 400;
                response.Message = "Failed to save data";
                return await Task.FromResult(response);
            }

            await _context.QuizRoomDatas.AddAsync(quizRoomData);
            await _context.SaveChangesAsync();

            response.Code = 200;
            return await Task.FromResult(response);
        }

        public override async Task<RoomResponse> GetAllRoomData(Data request, ServerCallContext context)
        {
            var response = new RoomResponse();

            response.Code = 200;
            IEnumerable<QuizRoomData> data = _context.QuizRoomDatas.ToImmutableList();
            response.Data = JsonConvert.SerializeObject(data);
            return await Task.FromResult<RoomResponse>(response);
        }

        public override async Task<RoomResponse> SaveParticipants(Data request, ServerCallContext context)
        {
            var response = new RoomResponse();
            var participants = JsonConvert.DeserializeObject<IEnumerable<QuizParticipant>>(request.Value);

            if(participants == null)
            {
                response.Code = 400;
                response.Message = "Error, Participants is null";
                return await Task.FromResult(response);
            }

            List<QuizParticipant> Entries = new();
            foreach(var participant in participants)
            {
                var p = new QuizParticipant
                {
                    QParticipantDesc = participant.QParticipantDesc,
                    QRoomId = participant.QRoomId,
                    UserId = participant.UserId,
                    Score = participant.Score,
                    QStartDate = participant.QStartDate,
                    QEndDate = participant.QEndDate,
                    QStatus = participant.QStatus,
                };
                var entry = _context.QuizParticipants.Add(p);
                var entity = entry.Entity;
                _context.SaveChanges();
                if (entry == null) continue;
                Entries.Add(entity);
                
            }
            response.Code = 200;
            response.Data = JsonConvert.SerializeObject(Entries);
            return await Task.FromResult(response);
        }

    }
}
