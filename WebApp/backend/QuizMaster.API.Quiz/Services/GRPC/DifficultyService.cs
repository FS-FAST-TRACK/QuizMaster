using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;

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

                if (difficulty == null || !difficulty.ActiveData)
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
            catch (Exception ex)
            {
                error.Code = 500;
                error.Message = ex.Message;
            }
            return await Task.FromResult(reply);
        }

        public override async Task<DifficultyByDescResponse> GetDifficultyByDesc(GetDifficultyByDescRequest request, ServerCallContext context)
        {
            var reply = new DifficultyByDescResponse();

            try
            {
                var checkDesc = await _quizRepository.GetDifficultyAsync(request.Desc);
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
                        reply.Id = checkDesc.Id;
                        checkDesc.ActiveData = true;
                        _quizRepository.UpdateDifficulty(checkDesc);
                        _quizRepository.SaveChangesAsync();

                    }
                }
            }
            catch (Exception)
            {
                reply.Code = 500;
            }
            return await Task.FromResult(reply);
        }

        public override async Task<CreateDifficultyResponse> CreateDifficulty(CreateDifficultyRequest request, ServerCallContext context)
        {
            var reply = new CreateDifficultyResponse();

            try
            {
                var difficulty = _mapper.Map<QuestionDifficulty>(request);
                await _quizRepository.AddDifficultyAsync(difficulty);
                await _quizRepository.SaveChangesAsync();

                reply.Code = 201;
                reply.Id = difficulty.Id;
                reply.QDifficultyDesc = difficulty.QDifficultyDesc;
            }
            catch (Exception)
            {
                reply.Code = 500;
            }

            return await Task.FromResult(reply);
        }

        public override async Task<DeleteDifficultyReply> DeleteDifficulty(GetDificultyRequest request, ServerCallContext context)
        {
            var reply = new DeleteDifficultyReply();

            var difficulty = _quizRepository.GetDifficultyAsync(request.Id).Result;
            if (difficulty == null || !difficulty.ActiveData)
            {
                reply.Code = 404;
            }
            else
            {
                difficulty.ActiveData = false;
                var isSuccess = _quizRepository.UpdateDifficulty(difficulty);

                if (!isSuccess)
                {
                    reply.Code = 500;
                }
                else
                {
                    reply.Code = 200;
                }
                await _quizRepository.SaveChangesAsync();
            }

            return await Task.FromResult(reply);
        }

        public override async Task<UpdateDifficultyResponse> UpdateDifficulty(UpdateDifficultyRequest request, ServerCallContext context)
        {
            var reply = new UpdateDifficultyResponse();
            var id = request.Id;
            var patch = JsonConvert.DeserializeObject<JsonPatchDocument<DifficultyCreateDto>>(request.Patch);

            var checkDifficultyId = _quizRepository.GetDifficultyAsync(id).Result;
            if(checkDifficultyId == null || !checkDifficultyId.ActiveData)
            {
                reply.Code = 404;
                return await Task.FromResult(reply);
            }

            var difficultyPatch = new DifficultyCreateDto();
            patch.ApplyTo(difficultyPatch);

            if(await _quizRepository.GetDifficultyAsync(difficultyPatch.QDifficultyDesc) != null)
            {
                reply.Code = 409;
                return await Task.FromResult(reply);
            }

            _mapper.Map(difficultyPatch, checkDifficultyId);

            var isSuccess = _quizRepository.UpdateDifficulty(checkDifficultyId);

            if(!isSuccess)
            {
                reply.Code = 500;
            }
            else 
            {
                await _quizRepository.SaveChangesAsync();

                reply.Code = 200;
                reply.Id = checkDifficultyId.Id;
                reply.QDifficultyDesc = checkDifficultyId.QDifficultyDesc;
            }

            return await Task.FromResult(reply);
        }

        //public override async Task<CreateDifficultyResponse> UpdateDifficulty(CreateDifficultyRequest request, ServerCallContext context)
        //{
        //    var reply = new CreateDifficultyResponse();

        //    var difficulty = JsonConvert.DeserializeObject<DifficultyCreateDto>(request.QDifficultyDesc);
        //    var checkDifficulty = _quizRepository.GetDifficultyAsync(difficulty.QDifficultyDesc).Result;
        //    if (checkDifficulty != null)
        //    {
        //        reply.Code = 400;
        //    }
        //    else
        //    {
        //        var difficultyFromRepo = _mapper.Map<QuestionDifficulty>(checkDifficulty);
        //        var isSuccess = _quizRepository.UpdateDifficulty(difficultyFromRepo);

        //        if (!isSuccess)
        //        {
        //            reply.Code = 500;
        //        }
        //        else
        //        {
        //            reply.Code = 200;
        //            reply.Id = difficultyFromRepo.Id;
        //            reply.QDifficultyDesc = difficultyFromRepo.QDifficultyDesc;
        //        }
        //        await _quizRepository.SaveChangesAsync();
        //    }

        //    return await Task.FromResult(reply);
        //}
    }
}
