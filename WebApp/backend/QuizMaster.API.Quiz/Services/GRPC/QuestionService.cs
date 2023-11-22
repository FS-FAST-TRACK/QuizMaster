using AutoMapper;
using Azure;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Models.ValidationModel;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class QuestionService : QuestionServices.QuestionServicesBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionDetailManager _questionDetailManager;
        private readonly IMapper _mapper;

        public QuestionService(IQuizRepository quizRepository, IQuestionDetailManager questionDetailManager,IMapper mapper)
        {
            _quizRepository = quizRepository;
            _questionDetailManager = questionDetailManager;
            _mapper = mapper;
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

            if( questionRepo != null && questionRepo.ActiveData ) 
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
            }

            if (!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            await _quizRepository.SaveChangesAsync();
            var questionDto = _mapper.Map<QuestionDto>(questionRepo);

            reply.Code = 200;
            reply.Questions = JsonConvert.SerializeObject(questionDto);

            return await Task.FromResult(reply);
        }

        public override async Task<QuestionResponse> DeleteQuestion(GetQuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();
            var question = await _quizRepository.GetQuestionAsync(request.Id);

            if(question == null || !question.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            question.ActiveData = false;
            var isSuccess = _quizRepository.UpdateQuestion(question);

            if (!isSuccess) 
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            reply.Code = 200;
            await _quizRepository.SaveChangesAsync();

            return await Task.FromResult(reply);
        }

        public override async Task<QuestionResponse> PatchQuestion(PatchQuestionRequest request, ServerCallContext context)
        {
            var reply = new QuestionResponse();

            var id = request.Id;
            var patch = JsonConvert.DeserializeObject<JsonPatchDocument<QuestionCreateDto>>(request.Patch);

            var question = await _quizRepository.GetQuestionAsync(id);

            if(question == null || !question.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            var questionPatch = _mapper.Map<QuestionCreateDto>(question);

            patch.ApplyTo(questionPatch);

            var category = await _quizRepository.GetCategoryAsync(questionPatch.QCategoryId);
            var difficulty = await _quizRepository.GetDifficultyAsync(questionPatch.QDifficultyId);
            var type = await _quizRepository.GetTypeAsync(questionPatch.QTypeId);


            // Guard if category, difficulty, and type is not found
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

                reply.Code = 200;
                reply.Questions = JsonConvert.SerializeObject(question);

                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Questions = ex.Message;
                return await Task.FromResult(reply);
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
