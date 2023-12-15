using AutoMapper;
using Azure;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Models.ValidationModel;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.API.Quiz.Services.Workers;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers;
using QuizMaster.Library.Common.Models;
using System.Text.RegularExpressions;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class QuestionService : QuestionServices.QuestionServicesBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionDetailManager _questionDetailManager;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;
        private readonly IMapper _mapper;
        private readonly QuizDataSynchronizationWorker _synchronizationWorker;

        public QuestionService(IQuizRepository quizRepository, IQuestionDetailManager questionDetailManager,IMapper mapper, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient, QuizDataSynchronizationWorker quizDataSynchronizationWorker)
        {
            _quizRepository = quizRepository;
            _questionDetailManager = questionDetailManager;
            _mapper = mapper;
            _quizAuditServiceClient = quizAuditServiceClient;
            _synchronizationWorker = quizDataSynchronizationWorker;
        }

        public override async Task<QuestionResponse> GetQuestions(QuestionRequest request, ServerCallContext context)
        {
            var resourceParameter = JsonConvert.DeserializeObject<QuestionResourceParameter>(request.Parameter);
            var questions = await _quizRepository.GetAllQuestionsAsync(resourceParameter);
            
            var serial = JsonConvert.SerializeObject(questions);
            var reply = new QuestionResponse();
            reply.Questions = serial;

            return await Task.FromResult(reply);
        }

        public override async Task<QuestionResponse> GetQuestion(GetQuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();

            var question = await _quizRepository.GetQuestionAsync(request.Id);
            var questionDetail = await _quizRepository.GetQuestionDetailsAsync(request.Id);

            if(question == null || !question.ActiveData) 
            {
                reply.Code = 404;
            }
            else
            {
                var questionDto = _mapper.Map<QuestionDto>(question);
                questionDto.Details = _mapper.Map<IEnumerable<QuestionDetailDto>>(questionDetail);
                reply.Code = 200;
                reply.Questions = JsonConvert.SerializeObject(questionDto);
            }

            return await Task.FromResult(reply);
        }

        public override async Task<QuestionResponse> AddQuestion(QuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();
            var question = JsonConvert.DeserializeObject<QuestionCreateDto>(request.Parameter);

            var questionRepo = await _quizRepository.GetQuestionAsync(question.QStatement, question.QDifficultyId,
                                                                        question.QTypeId, question.QCategoryId);

            if (questionRepo != null && questionRepo.ActiveData)
            {
                reply.Code = 409;
                return await Task.FromResult(reply);
            }

            bool isSuccess;

            if (questionRepo != null && !questionRepo.ActiveData)
            {
                questionRepo.ActiveData = true;
                isSuccess = _quizRepository.UpdateQuestion(questionRepo);
            }
            else
            {
                var category = await _quizRepository.GetCategoryAsync(question.QCategoryId);
                var difficulty = await _quizRepository.GetDifficultyAsync(question.QDifficultyId);
                var type = await _quizRepository.GetTypeAsync(question.QTypeId);

                questionRepo = _mapper.Map<Question>(question);

                questionRepo.QCategory = category;
                questionRepo.QDifficulty = difficulty;
                questionRepo.QType = type;

                var isQuestionAddedSuccessfully = await _quizRepository.AddQuestionAsync(questionRepo);
                var isDetailAddedSuccessfully = true;

                if (isQuestionAddedSuccessfully)
                {
                    isDetailAddedSuccessfully = await _questionDetailManager.AddQuestionDetailAsync(questionRepo, question.questionDetailCreateDtos);
                }

                isSuccess = isDetailAddedSuccessfully && isQuestionAddedSuccessfully;

                // Log the add question event
               LogCreateQuestionEvent(questionRepo, request, context);
            }

            if (!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            await _quizRepository.SaveChangesAsync();
            await _synchronizationWorker.Synchronize();
            var questionDto = _mapper.Map<QuestionDto>(questionRepo);

            reply.Code = 200;
            reply.Questions = JsonConvert.SerializeObject(questionDto);

            return await Task.FromResult(reply);
        }
        private void LogCreateQuestionEvent(Question addedQuestion, QuestionRequest request, ServerCallContext context)
        {
            // Capture the details of the user adding the question
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            var serializedQuestion = JsonConvert.SerializeObject(new
            {
                addedQuestion.Id,
                addedQuestion.QStatement,
                addedQuestion.QImage,
                addedQuestion.QAudio,
                addedQuestion.QTime,
                addedQuestion.QDifficultyId,
                addedQuestion.QCategoryId,
                addedQuestion.QTypeId,
                Details = addedQuestion.Details.Select(d => new
                {
                    d.Id,
                    d.QDetailDesc,
                    
                }).ToList()
            });



            // Construct the add question event
            var addQuestionEvent = new CreateQuestionEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Create Question",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Question added by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = "",
                NewValues = JsonConvert.SerializeObject(serializedQuestion),
            };

            var logRequest = new LogCreateQuestionEventRequest
            {
                Event = addQuestionEvent
            };

            try
            {
                // Make the gRPC call to log the add question event
                _quizAuditServiceClient.LogCreateQuestionEvent(logRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task<QuestionResponse> DeleteQuestion(GetQuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();
            var question = await _quizRepository.GetQuestionAsync(request.Id);

            if (question == null || !question.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

           

            // Log the delete question event
            LogDeleteQuestionEvent(question, context);

            question.ActiveData = false;
            var isSuccess = _quizRepository.UpdateQuestion(question);

            if (!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            

            reply.Code = 200;
            await _quizRepository.SaveChangesAsync();
            await _synchronizationWorker.Synchronize();

            return await Task.FromResult(reply);
        }

        private void LogDeleteQuestionEvent(Question deletedQuestion, ServerCallContext context)
        {

            // Capture the details of the user deleting the question
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            var serializedQuestion = JsonConvert.SerializeObject(new
            {
                deletedQuestion.Id,
                deletedQuestion.QStatement,
                deletedQuestion.QImage,
                deletedQuestion.QAudio,
                deletedQuestion.QTime,
                deletedQuestion.QDifficultyId,
                deletedQuestion.QCategoryId,
                deletedQuestion.QTypeId,
                Details = deletedQuestion.Details.Select(d => new
                {
                    d.Id,
                    d.QDetailDesc,

                }).ToList()
            });

            // Construct the delete event
            var deleteEvent = new DeleteQuestionEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Delete Question",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Question deleted by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(serializedQuestion),
                NewValues = ""
            };

            var logRequest = new LogDeleteQuestionEventRequest
            {
                Event = deleteEvent
            };

            try
            {
                // Make the gRPC call to log the delete question event
                _quizAuditServiceClient.LogDeleteQuestionEvent(logRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public override async Task<QuestionResponse> PatchQuestion(PatchQuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();

            var id = request.Id;
            var patch = JsonConvert.DeserializeObject<JsonPatchDocument<QuestionCreateDto>>(request.Patch);

            var question = await _quizRepository.GetQuestionAsync(id);

            if (question == null || !question.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            var oldQuestionState = _mapper.Map<QuestionCreateDto>(question);

            var questionPatch = _mapper.Map<QuestionCreateDto>(question);
            patch.ApplyTo(questionPatch);

            var category = await _quizRepository.GetCategoryAsync(questionPatch.QCategoryId);
            var difficulty = await _quizRepository.GetDifficultyAsync(questionPatch.QDifficultyId);
            var type = await _quizRepository.GetTypeAsync(questionPatch.QTypeId);

            // Guard if category, difficulty, and type are not found
            var result = ValidateCategoryDifficultyType(category, difficulty, type);
            if (!result.IsValid)
            {
                reply.Code = 400;
                reply.Questions = result.Error;
                return await Task.FromResult(reply);
            }

            if (await _quizRepository.GetQuestionAsync(questionPatch.QStatement, questionPatch.QDifficultyId,
                                                      questionPatch.QTypeId, questionPatch.QCategoryId) != null)
            {
                reply.Code = 409;
                return await Task.FromResult(reply);
            }

            _mapper.Map(questionPatch, question);

            try
            {
                _quizRepository.UpdateQuestion(question);
                await _quizRepository.SaveChangesAsync();

                // Log the update question event with both old and new values
                LogUpdateQuestionEvent(oldQuestionState, questionPatch, context);

                reply.Code = 200;
                reply.Questions = JsonConvert.SerializeObject(question);
                await _synchronizationWorker.Synchronize();
                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Questions = ex.Message;
                return await Task.FromResult(reply);
            }
        }

        private void LogUpdateQuestionEvent(QuestionCreateDto oldQuestionState, QuestionCreateDto newQuestionState, ServerCallContext context)
        {
            // Capture the details of the user updating the question
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;


            // Construct the update event
            var updateEvent = new UpdateQuestionEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Update Question",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Question updated by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(oldQuestionState),
                NewValues = JsonConvert.SerializeObject(newQuestionState)
            };

            var logRequest = new LogUpdateQuestionEventRequest
            {
                Event = updateEvent
            };

            try
            {
                // Make the gRPC call to log the update question event
                _quizAuditServiceClient.LogUpdateQuestionEvent(logRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private ValidationModel ValidateCategoryDifficultyType(QuestionCategory? category, QuestionDifficulty? difficulty, QuestionType? type)
        {
            var validationModel = new ValidationModel();

            validationModel.Error += ValidateItem(category, "Category");
            validationModel.Error += ValidateItem(difficulty, "Difficulty");
            validationModel.Error += ValidateItem(type, "Type");

            return validationModel;
        }

        private string ValidateItem(IEntity? item, string itemName)
        {
            return item == null || !item.ActiveData ? $"{itemName} is not found. " : "";
        }
    }
}
