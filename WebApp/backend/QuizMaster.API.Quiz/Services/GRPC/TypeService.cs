using AutoMapper;
using Grpc.Core;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class TypeService : QuizTypeService.QuizTypeServiceBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;

        public TypeService(IQuizRepository quizRepository, IMapper mapper)
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
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
            var checkAvailabity = _quizRepository.GetTypeAsync(request.QTypeDesc).Result;
            if(checkAvailabity != null && checkAvailabity.ActiveData)
            {
                reply.Code = 409;
                return await Task.FromResult(reply);
            }

            bool isSuccess;

            if(checkAvailabity != null && !checkAvailabity.ActiveData)
            {
                checkAvailabity.ActiveData = true;
                isSuccess =  _quizRepository.UpdateType(checkAvailabity);
                await _quizRepository.SaveChangesAsync();
                reply.Code = 200;
                reply.Type = _mapper.Map<TypeReply>(checkAvailabity);
            }
            else 
            {
                var createType = _mapper.Map<QuestionType>(request);
                isSuccess = await _quizRepository.AddTypeAsync(createType);
                await _quizRepository.SaveChangesAsync();
                reply.Code = 200;
                reply.Type = _mapper.Map<TypeReply>(createType);
            }

            if(!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }


            return await Task.FromResult(reply);
        }

        public override async Task<QuizTypeResponse> DeleteType(GetQuizTypeRequest request, ServerCallContext context)
        {
            var reply = new QuizTypeResponse();

            var type = _quizRepository.GetTypeAsync(request.Id).Result;

            if(type == null || !type.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            type.ActiveData = false;
            var isSuccess = _quizRepository.UpdateType(type);

            if(!isSuccess)
            {
                reply.Code = 500;
                return await Task.FromResult(reply);
            }

            await _quizRepository.SaveChangesAsync();
            reply.Code = 204;
            return await Task.FromResult(reply);
        }
    }
}
