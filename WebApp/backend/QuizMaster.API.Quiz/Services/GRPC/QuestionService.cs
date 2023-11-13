using AutoMapper;
using Azure;
using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Models.ValidationModel;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class QuestionService : QuestionServices.QuestionServicesBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuizRepository quizRepository, IMapper mapper)
        {
            _quizRepository = quizRepository;
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
            var questionDetail = await _quizRepository.GetQuestionDetailAsync(request.Id);

            if(question == null || !question.ActiveData) 
            {
                reply.Code = 404;
            }
            else
            {
                var questionDto = _mapper.Map<QuestionDto>(question);
                questionDto.Details = _mapper.Map<DetailDto>(questionDetail);
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

            var detail = _mapper.Map<QuestionDetail>(question);

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

                if (isQuestionAddedSuccessfully && type!.QDetailRequired)
                {
                    // Link the details to question. 
                    detail.Question = questionRepo;
                    // Created by UserId must be updated by the time we have access to tokens
                    detail.CreatedByUserId = 1;
                    isDetailAddedSuccessfully = await _quizRepository.AddQuestionDetailsAsync(detail);
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
            questionDto.Details = _mapper.Map<DetailDto>(detail);

            reply.Code = 200;
            reply.Questions = JsonConvert.SerializeObject(questionDto);

            return await Task.FromResult(reply);
        }
    }
}
