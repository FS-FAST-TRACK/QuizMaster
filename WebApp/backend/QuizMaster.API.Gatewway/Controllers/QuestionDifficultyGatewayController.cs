using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("api/gateway/question/difficulty")]
    public class QuestionDifficultyGatewayController : Controller
    {
        private readonly IMapper _mapper;
        private readonly GrpcChannel _channel;
        private readonly QuizDifficultyService.QuizDifficultyServiceClient _channelClient;

        public QuestionDifficultyGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service);
            _channelClient = new QuizDifficultyService.QuizDifficultyServiceClient(_channel);
        }

        [HttpGet("get_all_difficulty")]
        public async Task<IActionResult> GetAllDifficulty()
        {
            var request = new EmptyDifficultyRequest();
            var response = _channelClient.GetDificulties(request);
            var difficulties = new List<DifficultyDto>();
            while(await response.ResponseStream.MoveNext())
            {
                difficulties.Add(_mapper.Map<DifficultyDto>(response.ResponseStream.Current));
            }
            return Ok(difficulties);
        }
    }
}
