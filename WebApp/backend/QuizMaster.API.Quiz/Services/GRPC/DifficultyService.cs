using AutoMapper;
using Grpc.Core;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class DifficultyService : QuizDifficultyService.QuizDifficultyServiceBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public DifficultyService(IQuizRepository quizRepository, IMapper mapper)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
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
    }
}
