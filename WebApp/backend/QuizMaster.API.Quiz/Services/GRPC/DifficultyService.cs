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

        public override async Task<DifficultyOrNotFound> GetDificulty(GetDificultyRequest request, ServerCallContext context)
        {
            var success = new DificultiesReply();
            var error = new NotFoundDifficulty();
            var reply = new DifficultyOrNotFound();

            try
            {
                var difficulty = await _quizRepository.GetDifficultyAsync(request.Id);

                if (difficulty == null)
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
            catch(Exception ex)
            {
                error.Code = 500;
                error.Message = ex.Message;
            }
            return await Task.FromResult(reply);
        }
    }
}
