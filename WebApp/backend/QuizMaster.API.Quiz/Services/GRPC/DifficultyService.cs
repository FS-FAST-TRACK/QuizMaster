using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers;
using QuizMaster.Library.Common.Helpers.Quiz;
using System.Security.Policy;

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

        public override async Task<DifficultyResponse> GetAllDifficulty(DifficultyRequest request, ServerCallContext context)
        {
            var reply = new DifficultyResponse();
            
            var difficulties = await _quizRepository.GetAllDifficultiesAsync();

            reply.Content = JsonConvert.SerializeObject(difficulties);

            reply.Code = 200;
            reply.Type = "Difficulties";
            return reply;
        }

        public override async Task<DifficultyResponse> GetDifficulties(DifficultyRequest request, ServerCallContext context)
        {
            var reply = new DifficultyResponse();
            var resourceParameter = new DifficultyResourceParameter();
            try
            {
                resourceParameter = JsonConvert.DeserializeObject<DifficultyResourceParameter>(request.Content);
                if(resourceParameter == null ) {
                    throw new Exception("GRPC request content cannot be deserialized in to Tuple<int, JsonPatchDocument<DifficultyCreateDto>>.");
                }
            }
            catch (Exception ex)
            {
                reply.Content = ex.Message;
                reply.Code = 500;
                reply.Type = "string";
                return reply;
            }

            var difficulties = await _quizRepository.GetAllDifficultiesAsync(resourceParameter);

            var paginationMetadata = difficulties!.GeneratePaginationMetadata(null, null);

            reply.Content = JsonConvert.SerializeObject(new Tuple<IEnumerable<DifficultyDto>, Dictionary<string, object?>>(difficulties, paginationMetadata));
            reply.Code = 200;
            reply.Type = "pagedDifficulties";
            return reply;
        }

        public override async Task<DifficultyResponse> GetDifficulty(DifficultyRequest request, ServerCallContext context)
        {

            var reply = new DifficultyResponse();
            int id = 0;
            try
            {
                id = JsonConvert.DeserializeObject<int>(request.Content);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }

            try
            {
                var difficulty = await _quizRepository.GetDifficultyAsync(id);

                if (difficulty == null || !difficulty.ActiveData)
                {
                    reply.Code = 404;
                    reply.Content = "Difficulty not found";
                    reply.Type = "string";
                }
                else
                {
                    reply.Content = JsonConvert.SerializeObject(difficulty);
                    reply.Code = 200;
                    reply.Type = "difficulty";

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }
            return reply;
        }

        public override async Task<DifficultyResponse> GetDifficultyByDesc(DifficultyRequest request, ServerCallContext context)
        {
            var reply = new DifficultyResponse();
            string description = "";
            try
            {
                description = JsonConvert.DeserializeObject<string>(request.Content)!;
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }

            try
            {
                var checkDesc = await _quizRepository.GetDifficultyAsync(description);
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
                        reply.Content = JsonConvert.SerializeObject(checkDesc);
                        reply.Type = "difficulty";
                        checkDesc.ActiveData = true;
                        _quizRepository.UpdateDifficulty(checkDesc);
                        await _quizRepository.SaveChangesAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }
            return reply;
        }



        public override async Task<DifficultyResponse> DeleteDifficulty(DifficultyRequest request, ServerCallContext context)
        {
            var reply = new DifficultyResponse();
            int id = 0;
            try
            {
                id = JsonConvert.DeserializeObject<int>(request.Content);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }

            var difficulty = await _quizRepository.GetDifficultyAsync(id);
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

                // Set updatedby userId and DateUpdated to latest
                difficulty.DateUpdated = DateTime.UtcNow;
                difficulty.UpdatedByUserId = int.Parse(userId!);

                var isSuccess = _quizRepository.UpdateDifficulty(difficulty);

                if (!isSuccess)
                {
                    reply.Code = 500;
                    reply.Content = "Failed to update delete functionality";
                    reply.Type = "string";
                }
                else
                {
                    // Log the delete difficulty event with the old values
                    LogDeleteQuizDifficultyEvent(deletedDifficulty, context);
                    reply.Code = 200;
                    reply.Content = "Succesfully Deleted Difficulty";
                    reply.Type = "string";
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


        public override async Task<DifficultyResponse> UpdateDifficulty(DifficultyRequest request, ServerCallContext context)
        {
            var reply = new DifficultyResponse();
            int id = 0;
            var patch = new JsonPatchDocument<DifficultyCreateDto>();
            var updateContent = new Tuple<int, JsonPatchDocument<DifficultyCreateDto>>(id, patch);
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                updateContent = JsonConvert.DeserializeObject<Tuple<int, JsonPatchDocument<DifficultyCreateDto>>>(request.Content);
                if (updateContent == null)
                {
                    throw new Exception("GRPC request content cannot be deserialized in to Tuple<int, JsonPatchDocument<DifficultyCreateDto>>.");
                }
                if (userId == null)
                {
                    throw new Exception("User Id is not found in the GRPC context.");

                }
                id = updateContent.Item1;
                patch = updateContent.Item2;
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }



            var checkDifficultyId = await _quizRepository.GetDifficultyAsync(id);

            if (checkDifficultyId == null || !checkDifficultyId.ActiveData)
            {
                reply.Code = 404;
                reply.Content = "Difficulty is not found";
                reply.Type = "string";
                return reply;
            }

            // Capture the old difficulty state manually
            var oldDifficulty = new DifficultyCreateDto
            {
                QDifficultyDesc = checkDifficultyId.QDifficultyDesc,
                // Map other properties as needed
            };

            try
            {
            var difficultyPatch = _mapper.Map<DifficultyCreateDto>(checkDifficultyId);
            patch.ApplyTo(difficultyPatch);

                // Check if the QDifficultyDesc already exists
                var existingDifficulty = await _quizRepository.GetDifficultyAsync(difficultyPatch.QDifficultyDesc);
                if (existingDifficulty != null && existingDifficulty.Id != id)
                {
                    reply.Code = 409;
                    reply.Content = $"Difficulty \'{difficultyPatch.QDifficultyDesc}\' already exist.";
                    return reply;
                }



                // Update Properties
                _mapper.Map(difficultyPatch, checkDifficultyId);

                // Update other properties as needed
                checkDifficultyId.UpdatedByUserId = int.Parse(userId!);
                checkDifficultyId.DateUpdated = DateTime.UtcNow;

                var isSuccess = _quizRepository.UpdateDifficulty(checkDifficultyId);

                if (!isSuccess)
                {
                    reply.Code = 500;
                    reply.Content = "Failed to update Difficulty";
                    reply.Type = "string";
                }
                else
                {
                    await _quizRepository.SaveChangesAsync();

                    reply.Code = 200;
                    reply.Content = JsonConvert.SerializeObject(checkDifficultyId);

                    // Log the update difficulty event with both old and new values
                    LogUpdateQuizDifficultyEvent(oldDifficulty, difficultyPatch, context);
                }
            }
            catch (Exception)
            {

                reply.Content = "THINGS ARE BAD HERE";
                reply.Type = "error";
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
        public override async Task<DifficultyResponse> CreateDifficulty(DifficultyRequest request, ServerCallContext context)
        {
            var reply = new DifficultyResponse();
            var difficulty = new DifficultyCreateDto();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                difficulty = JsonConvert.DeserializeObject<DifficultyCreateDto>(request.Content);
                if (difficulty == null)
                {
                    throw new Exception("Failed to deserialize GRPC request content into DifficultyCreateDto");
                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }
            // Check if difficulty description already exist
            var difficultyFromRepo = await _quizRepository.GetDifficultyAsync(difficulty!.QDifficultyDesc);

            if (difficultyFromRepo != null && difficultyFromRepo.ActiveData)
            {
                reply.Code = 409;
                reply.Content = "Difficulty already exist";
                reply.Type = "string";
            }

            bool isSuccess;

            // If difficulty is not null and not active, we set active to true and update the difficulty
            if (difficultyFromRepo != null && !difficultyFromRepo.ActiveData)
            {
                difficultyFromRepo.ActiveData = true;
                // update difficulty
                _mapper.Map(difficulty, difficultyFromRepo);

                // udpate necessary properties
                difficultyFromRepo.DateUpdated = DateTime.UtcNow;
                difficultyFromRepo.UpdatedByUserId = int.Parse(userId!);

                isSuccess = _quizRepository.UpdateDifficulty(difficultyFromRepo);
            }
            // else, we create new difficulty
            else
            {
                difficultyFromRepo = _mapper.Map<QuestionDifficulty>(difficulty);
                difficultyFromRepo.CreatedByUserId = int.Parse(userId!);
                difficultyFromRepo.DateCreated = DateTime.UtcNow;

                isSuccess = await _quizRepository.AddDifficultyAsync(difficultyFromRepo);
            }


            // Check if update or create is not access 
            if (!isSuccess)
            {
                reply.Content = "Failed to create difficulty.";
                reply.Type = "string";
                reply.Code = 500;
                return reply;
            }
            reply.Content = JsonConvert.SerializeObject(difficultyFromRepo);
            reply.Type = "difficulty";
            reply.Code = 201;

            await _quizRepository.SaveChangesAsync();
            return reply;
        }
    }
}
