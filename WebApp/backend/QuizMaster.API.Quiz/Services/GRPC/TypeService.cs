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
    public class TypeService : QuizTypeService.QuizTypeServiceBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;


        public TypeService(IQuizRepository quizRepository, IMapper mapper, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
            _quizAuditServiceClient = quizAuditServiceClient;
        }

        public override async Task GetAllTypes(EmptyTypeRequest request, IServerStreamWriter<TypeReply> responseStream, ServerCallContext context)
        {
            var reply = new TypeReply();
            foreach (var difficulty in _quizRepository.GetAllTypesAsync().Result)
            {
                reply.Id = difficulty.Id;
                reply.QTypeDesc = difficulty.QTypeDesc;
                reply.QDetailRequired = difficulty.QDetailRequired;

                await responseStream.WriteAsync(reply);
            }
        }

        public override async Task<QuizTypeResponse> GetQuizType(GetQuizTypeRequest request, ServerCallContext context)
        {
            var reply = new QuizTypeResponse();

            var type = _quizRepository.GetTypeAsync(request.Id).Result;
            if(type == null)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            reply.Code = 200;
            reply.Type = _mapper.Map<TypeReply>(type);

            return await Task.FromResult(reply);
        }

        public override async Task<QuizTypeResponse> AddQuizType(AddQuisTypeRequest request, ServerCallContext context)
        {
            var reply = new QuizTypeResponse();
            var checkAvailability = _quizRepository.GetTypeAsync(request.QTypeDesc).Result;

            if (checkAvailability != null && checkAvailability.ActiveData)
            {
                reply.Code = 409;
                return await Task.FromResult(reply);
            }

            bool isSuccess;

            if (checkAvailability != null && !checkAvailability.ActiveData)
            {
                checkAvailability.ActiveData = true;
                isSuccess = _quizRepository.UpdateType(checkAvailability);
                await _quizRepository.SaveChangesAsync();
                reply.Code = 200;
                reply.Type = _mapper.Map<TypeReply>(checkAvailability);

                // Log the create question type event
                LogCreateQuestionTypeEvent(checkAvailability, context);
            }
            else
            {
                var createType = _mapper.Map<QuestionType>(request);
                isSuccess = await _quizRepository.AddTypeAsync(createType);
                await _quizRepository.SaveChangesAsync();
                reply.Code = 200;
                reply.Type = _mapper.Map<TypeReply>(createType);

                // Log the create question type event
                LogCreateQuestionTypeEvent(createType, context);
            }

            if (!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            return await Task.FromResult(reply);
        }

        private void LogCreateQuestionTypeEvent(QuestionType addedQuestionType, ServerCallContext context)
        {
            // Capture the details of the user adding the question
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;


            var serializedQuestionType = JsonConvert.SerializeObject(new
            { 
                addedQuestionType.Id,
                addedQuestionType.QTypeDesc,
                addedQuestionType.QDetailRequired,
                addedQuestionType.ActiveData
            });



            // Construct the add question event
            var addQuestionTypeEvent = new CreateQuestionTypeEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Create Question Type",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Question type added by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = "",
                NewValues = JsonConvert.SerializeObject(serializedQuestionType),
            };

            var logRequest = new LogCreateQuestionTypeEventRequest
            {
                Event = addQuestionTypeEvent
            };

            try
            {
                // Make the gRPC call to log the add question event
                _quizAuditServiceClient.LogCreateQuestionTypeEvent(logRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task<QuizTypeResponse> DeleteType(GetQuizTypeRequest request, ServerCallContext context)
        {
            var reply = new QuizTypeResponse();

            var type = _quizRepository.GetTypeAsync(request.Id).Result;

            if (type == null || !type.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }
            var serializedQuestionType = JsonConvert.SerializeObject(new
            {
                type.Id,
                type.QTypeDesc,
                type.QDetailRequired,
                type.ActiveData
            });


            // Capture the details of the user deleting the question type
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // Construct the delete question type event
            var deleteQuestionTypeEvent = new DeleteQuestionTypeEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Delete Question Type",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Question type deleted by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(serializedQuestionType),
                NewValues = "",
            };

            var logRequest = new LogDeleteQuestionTypeEventRequest
            {
                Event = deleteQuestionTypeEvent
            };

            try
            {
                // Make the gRPC call to log the delete question type event
                _quizAuditServiceClient.LogDeleteQuestionTypeEvent(logRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            type.ActiveData = false;
            var isSuccess = _quizRepository.UpdateType(type);

            if (!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            await _quizRepository.SaveChangesAsync();
            reply.Code = 204;
            return await Task.FromResult(reply);
        }


        public override async Task<QuizTypeResponse> UpdateType(UpdateTypeRequest request, ServerCallContext context)
        {
            var reply = new QuizTypeResponse();
            var id = request.Id;
            var patch = JsonConvert.DeserializeObject<JsonPatchDocument<TypeCreateDto>>(request.Patch);

            var checkTypeId = _quizRepository.GetTypeAsync(id).Result;
            if (checkTypeId == null || !checkTypeId.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            var typePatch = new TypeCreateDto();

            _mapper.Map(checkTypeId, typePatch);

            patch.ApplyTo(typePatch);

            if (_quizRepository.GetTypeAsync(typePatch.QTypeDesc).Result != null)
            {
                reply.Code = 409;
                return await Task.FromResult(reply);
            }

            // Capture the old type state
            var oldType = _mapper.Map<QuestionType, TypeCreateDto>(checkTypeId);

            _mapper.Map(typePatch, checkTypeId);

            var isSuccess = _quizRepository.UpdateType(checkTypeId);
            if (!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            await _quizRepository.SaveChangesAsync();
            reply.Type = _mapper.Map<TypeReply>(checkTypeId);

            // Log the update question type event with both old and new values
            LogUpdateQuestionTypeEvent(oldType, typePatch, context);

            return await Task.FromResult(reply);
        }

        private void LogUpdateQuestionTypeEvent(TypeCreateDto oldType, TypeCreateDto newType, ServerCallContext context)
        {
            // Capture the details of the user updating the question type
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // Construct the update question type event
            var updateQuestionTypeEvent = new UpdateQuestionTypeEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Update Question Type",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Question type updated by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(oldType),
                NewValues = JsonConvert.SerializeObject(newType),
            };

            var logRequest = new LogUpdateQuestionTypeEventRequest
            {
                Event = updateQuestionTypeEvent
            };

            try
            {
                // Make the gRPC call to log the update question type event
                _quizAuditServiceClient.LogUpdateQuestionTypeEvent(logRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
