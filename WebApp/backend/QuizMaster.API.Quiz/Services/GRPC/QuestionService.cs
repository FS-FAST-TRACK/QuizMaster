using AutoMapper;
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
using QuizMaster.Library.Common.Helpers.Quiz;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class QuestionService : QuestionServices.QuestionServicesBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionDetailManager _questionDetailManager;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;
        private readonly IMapper _mapper;
        private readonly QuizDataSynchronizationWorker _synchronizationWorker;

        public QuestionService(IQuizRepository quizRepository, IQuestionDetailManager questionDetailManager, IMapper mapper, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient, QuizDataSynchronizationWorker quizDataSynchronizationWorker)
        {
            _quizRepository = quizRepository;
            _questionDetailManager = questionDetailManager;
            _mapper = mapper;
            _quizAuditServiceClient = quizAuditServiceClient;
            _synchronizationWorker = quizDataSynchronizationWorker;
        }

        public override async Task<QuestionResponse> GetAllQuestions(QuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();

            var questions = await _quizRepository.GetAllQuestionsAsync();

            reply.Content = JsonConvert.SerializeObject(questions);

            reply.Code = 200;
            reply.Type = "questions";
            return reply;
        }

        public override async Task<QuestionResponse> GetQuestions(QuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();
            var resourceParameter = new QuestionResourceParameter();
            try
            {
                resourceParameter = JsonConvert.DeserializeObject<QuestionResourceParameter>(request.Content);
                if (resourceParameter == null)
                {
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

            var questions = await _quizRepository.GetAllQuestionsAsync(resourceParameter);

            var paginationMetadata = questions!.GeneratePaginationMetadata(null, null);

            reply.Content = JsonConvert.SerializeObject(new Tuple<IEnumerable<Question>, Dictionary<string, object?>>(questions, paginationMetadata));
            reply.Code = 200;
            reply.Type = "pagedQuestions";
            return reply;

        }

        public override async Task<QuestionResponse> GetQuestion(QuestionRequest request, ServerCallContext context)
        {

            var reply = new QuestionResponse();
            int id = request.Id;


            try
            {
                var question = await _quizRepository.GetQuestionAsync(id);

                if (question == null || !question.ActiveData)
                {
                    reply.Code = 404;
                    reply.Content = "Difficulty not found";
                    reply.Type = "string";
                    return reply;
                }

                var questionDetail = await _quizRepository.GetQuestionDetailsAsync(request.Id);
                var questionDto = _mapper.Map<QuestionDto>(question);
                questionDto.Details = _mapper.Map<IEnumerable<QuestionDetailDto>>(questionDetail);


                reply.Content = JsonConvert.SerializeObject(questionDto);
                reply.Code = 200;
                reply.Type = "question";

                return reply;
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string"; return reply;
            }



        }

        public override async Task<QuestionResponse> AddQuestion(QuestionRequest request, ServerCallContext context)
        {

            var reply = new QuestionResponse();
            var question = new QuestionCreateDto();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                question = JsonConvert.DeserializeObject<QuestionCreateDto>(request.Content);
                if (question == null)
                {
                    throw new Exception("Failed to deserialize GRPC request content into QuestionCreateDto.");
                }
                if (userId == null)
                {
                    throw new Exception("Create category requires UserId to be not null.");

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }

            // Check if question already exist
            var questionFromRepo = await _quizRepository.GetQuestionAsync(_mapper.Map<Question>(question));

            if (questionFromRepo != null && questionFromRepo.ActiveData)
            {
                reply.Code = 409;
                reply.Content = "Question already exist";
                reply.Type = "string";
                return reply;
            }

            bool isSuccess;

            // If question is not null and not active, we set active to true and update the question
            if (questionFromRepo != null && !questionFromRepo.ActiveData)
            {
                questionFromRepo.ActiveData = true;
                // update category
                _mapper.Map(question, questionFromRepo);

                // udpate necessary properties
                questionFromRepo.DateUpdated = DateTime.Now;
                questionFromRepo.UpdatedByUserId = int.Parse(userId!);

                isSuccess = _quizRepository.UpdateQuestion(questionFromRepo);

                await _quizRepository.SaveChangesAsync();

                LogUpdateQuestionEvent(question, _mapper.Map<QuestionCreateDto>(questionFromRepo), context);
            }
            // else, we create new category
            else
            {
                var category = await _quizRepository.GetCategoryAsync(question.QCategoryId);
                var difficulty = await _quizRepository.GetDifficultyAsync(question.QDifficultyId);
                var type = await _quizRepository.GetTypeAsync(question.QTypeId);

                questionFromRepo = _mapper.Map<Question>(question);

                questionFromRepo.QCategory = category;
                questionFromRepo.QDifficulty = difficulty;
                questionFromRepo.QType = type;

                var isQuestionAddedSuccessfully = await _quizRepository.AddQuestionAsync(questionFromRepo);
                var isDetailAddedSuccessfully = true;

                if (isQuestionAddedSuccessfully)
                {
                    isDetailAddedSuccessfully = await _questionDetailManager.AddQuestionDetailAsync(questionFromRepo, question.questionDetailCreateDtos);
                }

                await _quizRepository.SaveChangesAsync();

                isSuccess = isDetailAddedSuccessfully && isQuestionAddedSuccessfully;

                // Log the add question event
                LogCreateQuestionEvent(questionFromRepo, request, context);

            }


            // Check if update or create is not access 
            if (!isSuccess)
            {
                reply.Content = "Failed to create question.";
                reply.Type = "string";
                reply.Code = 500;
                return reply;
            }

            

            // Log changes after success full saving changes 

            var questionDto = _mapper.Map<QuestionDto>(questionFromRepo);

            reply.Content = JsonConvert.SerializeObject(questionDto);
            reply.Type = "question";
            reply.Code = 201;

            return reply;
        }
        private void LogCreateQuestionEvent(Question addedQuestion, QuestionRequest request, ServerCallContext context)
        {
            // Capture the details of the user adding the question
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;
            try
            {
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
                }; try
                {
                    // Make the gRPC call to log the add question event
                    _quizAuditServiceClient.LogCreateQuestionEvent(logRequest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public override async Task<QuestionResponse> DeleteQuestion(QuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();
            int id = request.Id;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                if (userId == null)
                {
                    throw new Exception("Delete question requires UserId to be not null.");

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }

            var question = await _quizRepository.GetQuestionAsync(id);
            if (question == null || !question.ActiveData)
            {
                reply.Code = 404;
                reply.Type = "string";
                reply.Content = "Question is not found.";

            }
            else
            {


                // Set ActiveData to false to "soft delete" the category
                question.ActiveData = false;

                // Set updatedby userId and DateUpdated to latest
                question.DateUpdated = DateTime.Now;
                question.UpdatedByUserId = int.Parse(userId!);

                var isSuccess = _quizRepository.UpdateQuestion(question);

                if (!isSuccess)
                {
                    reply.Code = 500;
                    reply.Content = "Failed to delete question. Database throws an error.";
                    reply.Type = "string";
                    return reply;
                }


                reply.Code = 200;
                reply.Content = "Succesfully Deleted Question.";
                reply.Type = "string";


                await _quizRepository.SaveChangesAsync();

                // Log the delete category event with the old values
                LogDeleteQuestionEvent(question, context);
            }

            return reply;
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


        public override async Task<QuestionResponse> PatchQuestion(QuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();
            int id = request.Id;
            var patch = new JsonPatchDocument<QuestionCreateDto>();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                patch = JsonConvert.DeserializeObject<JsonPatchDocument<QuestionCreateDto>>(request.Content);
                if (patch == null)
                {
                    throw new Exception("GRPC request content cannot be deserialized in to JsonPatchDocument<QuestionCreateDto>.");
                }
                if (userId == null)
                {
                    throw new Exception("User Id is not found in the GRPC context.");

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }



            var checkQuestionId = await _quizRepository.GetQuestionAsync(id);

            if (checkQuestionId == null || !checkQuestionId.ActiveData)
            {
                reply.Code = 404;
                reply.Content = "Question is not found";
                reply.Type = "string";
                return reply;
            }

            // Capture the old category state manually

            var oldQuestionState = _mapper.Map<QuestionCreateDto>(checkQuestionId);

            try
            {
                // Apply changes
                var questionPatch = _mapper.Map<QuestionCreateDto>(checkQuestionId);
                patch.ApplyTo(questionPatch);
                _mapper.Map(questionPatch, checkQuestionId);

                // Check if the duolicate question  exists
                var existingCategory = await _quizRepository.GetQuestionAsync(checkQuestionId);
                if (existingCategory != null && existingCategory.Id != id)
                {
                    reply.Code = 409;
                    reply.Content = "Question already exist.";
                    return reply;
                }


                // Update other properties as needed
                checkQuestionId.UpdatedByUserId = int.Parse(userId!);
                checkQuestionId.DateUpdated = DateTime.Now;

                var isSuccess = _quizRepository.UpdateQuestion(checkQuestionId);

                if (!isSuccess)
                {
                    reply.Code = 500;
                    reply.Content = "Failed to update Question";
                    reply.Type = "string";
                    return reply;
                }
                await _quizRepository.SaveChangesAsync();

                reply.Code = 200;
                reply.Content = JsonConvert.SerializeObject(checkQuestionId);
                reply.Type = "question";
                await _quizRepository.SaveChangesAsync();
                LogUpdateQuestionEvent(oldQuestionState, questionPatch, context);
                return reply;

            }
            catch (Exception)
            {

                reply.Content = "Something went wrong.";
                reply.Type = "string";
            }

            return reply;

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
