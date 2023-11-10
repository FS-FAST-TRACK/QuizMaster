using AutoMapper;
using Azure;
using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;

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
    }
}
