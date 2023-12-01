using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class DifficultyService : QuizDifficultyService.QuizDifficultyServiceBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;

        public DifficultyService(IQuizRepository quizRepository, IMapper mapper, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _quizAuditServiceClient = quizAuditServiceClient;
        }

        public override async Task GetDificulties(EmptyDifficultyRequest request, IServerStreamWriter<DificultiesReply> responseStream, ServerCallContext context)
        {
            var reply = new DificultiesReply();
            foreach (var difficulty in _quizRepository.GetAllDifficultiesAsync().Result)
            {
                reply.Id = difficulty.Id;
                reply.QDifficultyDesc = difficulty.QDifficultyDesc;
                await responseStream.WriteAsync(reply);
            }
        }

        public override async Task<DifficultyOrNotFound> GetDificulty(GetDificultyRequest request, ServerCallContext context)
        {
            var success = new DificultiesReply();
            var error = new NotFoundDifficulty();
            var reply = new DifficultyOrNotFound();

            try
            {
                var difficulty = await _quizRepository.GetDifficultyAsync(request.Id);

                if (difficulty == null || !difficulty.ActiveData)
                {
                    error.Code = 404;
                    error.Message = "Difficulty not found";
                    reply.NotFoundDifficulty = error;
                }
                else
                {
                    success.QDifficultyDesc = difficulty.QDifficultyDesc;
                    success.Id = difficulty.Id;

                    reply.DificultiesReply = success;
                }
            }
            catch (Exception ex)
            {
                error.Code = 500;
                error.Message = ex.Message;
            }
            return await Task.FromResult(reply);
        }

        public override async Task<DifficultyByDescResponse> GetDifficultyByDesc(GetDifficultyByDescRequest request, ServerCallContext context)
        {
            var reply = new DifficultyByDescResponse();

            try
            {
                var checkDesc = await _quizRepository.GetDifficultyAsync(request.Desc);
                if (checkDesc == null)
                {
                    reply.Code = 400;
                }
                else
                {
                    if (checkDesc.ActiveData)
                    {
                        reply.Code = 200;

                    }
                    else
                    {
                        reply.Code = 201;
                        reply.Id = checkDesc.Id;
                        checkDesc.ActiveData = true;
                        _quizRepository.UpdateDifficulty(checkDesc);
                        _quizRepository.SaveChangesAsync();

                    }
                }
            }
            catch (Exception)
            {
                reply.Code = 500;
            }
            return await Task.FromResult(reply);
        }



        public override async Task<DeleteDifficultyReply> DeleteDifficulty(GetDificultyRequest request, ServerCallContext context)
        {
            var reply = new DeleteDifficultyReply();

            var difficulty = await _quizRepository.GetDifficultyAsync(request.Id);
            if (difficulty == null || !difficulty.ActiveData)
            {
                reply.Code = 404;
            }
            else
            {
                // Capture the details of the user attempting to delete the difficulty
                var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                // Capture the current state of the difficulty for audit logging
                var deletedDifficulty = new DifficultyCreateDto
                {
                    QDifficultyDesc = difficulty.QDifficultyDesc,
                    // Include other properties as needed
                };

                // Set ActiveData to false to "soft delete" the difficulty
                difficulty.ActiveData = false;
                var isSuccess = _quizRepository.UpdateDifficulty(difficulty);

                if (!isSuccess)
                {
                    reply.Code = 500;
                }
                else
                {
                    // Log the delete difficulty event with the old values
                    LogDeleteQuizDifficultyEvent(deletedDifficulty, context);

                    reply.Code = 200;
                }

                await _quizRepository.SaveChangesAsync();
            }

            return reply;
        }

        private void LogDeleteQuizDifficultyEvent(DifficultyCreateDto deletedDifficulty, ServerCallContext context)
        {
            // Capture the details of the user deleting the difficulty
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // Construct the delete event
            var deleteEvent = new DeleteQuizDifficultyEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Delete Difficulty",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Difficulty deleted by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(deletedDifficulty),
                NewValues = "", // No new values for a deletion
            };

            var logRequest = new LogDeleteQuizDifficultyEventRequest
            {
                Event = deleteEvent
            };

            try
            {
                // Make the gRPC call to log the delete difficulty event
                _quizAuditServiceClient.LogDeleteQuizDifficultyEvent(logRequest);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the gRPC call
                // Handle the exception appropriately based on your application's needs
            }
        }


        public override async Task<UpdateDifficultyResponse> UpdateDifficulty(UpdateDifficultyRequest request, ServerCallContext context)
        {
            var reply = new UpdateDifficultyResponse();
            var id = request.Id;
            var checkDifficultyId = await _quizRepository.GetDifficultyAsync(id);

            if (checkDifficultyId == null || !checkDifficultyId.ActiveData)
            {
                reply.Code = 404;
                return reply;
            }

            // Capture the old difficulty state manually
            var oldDifficulty = new DifficultyCreateDto
            {
                QDifficultyDesc = checkDifficultyId.QDifficultyDesc,
                // Map other properties as needed
            };

            var patch = JsonConvert.DeserializeObject<JsonPatchDocument<DifficultyCreateDto>>(request.Patch);

            var difficultyPatch = new DifficultyCreateDto();
            patch.ApplyTo(difficultyPatch);

            // Check if the QDifficultyDesc already exists
            var existingDifficulty = await _quizRepository.GetDifficultyAsync(difficultyPatch.QDifficultyDesc);
            if (existingDifficulty != null && existingDifficulty.Id != id)
            {
                reply.Code = 409;
                return reply;
            }

            // Update properties manually
            checkDifficultyId.QDifficultyDesc = difficultyPatch.QDifficultyDesc;
            // Update other properties as needed

            var isSuccess = _quizRepository.UpdateDifficulty(checkDifficultyId);

            if (!isSuccess)
            {
                reply.Code = 500;
            }
            else
            {
                await _quizRepository.SaveChangesAsync();

                reply.Code = 200;
                reply.Id = checkDifficultyId.Id;
                reply.QDifficultyDesc = checkDifficultyId.QDifficultyDesc;

                // Log the update difficulty event with both old and new values
                LogUpdateQuizDifficultyEvent(oldDifficulty, difficultyPatch, context);
            }

            return reply;
        }



        private void LogUpdateQuizDifficultyEvent(DifficultyCreateDto oldDifficulty, DifficultyCreateDto newDifficulty, ServerCallContext context)
        {
            // Capture the details of the user updating the difficulty
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // Construct the update event
            var updateEvent = new UpdateQuizDifficultyEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Update Difficulty",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Difficulty updated by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(oldDifficulty),
                NewValues = JsonConvert.SerializeObject(newDifficulty),
            };

            var logRequest = new LogUpdateQuizDifficultyEventRequest
            {
                Event = updateEvent
            };

            try
            {
                // Make the gRPC call to log the update difficulty event
                _quizAuditServiceClient.LogUpdateQuizDifficultyEvent(logRequest);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the gRPC call
            }
        }
        //public override async Task<CreateDifficultyResponse> UpdateDifficulty(CreateDifficultyRequest request, ServerCallContext context)
        //{
        //    var reply = new CreateDifficultyResponse();

        //    var difficulty = JsonConvert.DeserializeObject<DifficultyCreateDto>(request.QDifficultyDesc);
        //    var checkDifficulty = _quizRepository.GetDifficultyAsync(difficulty.QDifficultyDesc).Result;
        //    if (checkDifficulty != null)
        //    {
        //        reply.Code = 400;
        //    }
        //    else
        //    {
        //        var difficultyFromRepo = _mapper.Map<QuestionDifficulty>(checkDifficulty);
        //        var isSuccess = _quizRepository.UpdateDifficulty(difficultyFromRepo);

        //        if (!isSuccess)
        //        {
        //            reply.Code = 500;
        //        }
        //        else
        //        {
        //            reply.Code = 200;
        //            reply.Id = difficultyFromRepo.Id;
        //            reply.QDifficultyDesc = difficultyFromRepo.QDifficultyDesc;
        //        }
        //        await _quizRepository.SaveChangesAsync();
        //    }

        //    return await Task.FromResult(reply);
        //}
    }
}
