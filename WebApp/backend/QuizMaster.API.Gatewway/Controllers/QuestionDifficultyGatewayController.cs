using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.Library.Common.Models;

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

        [HttpGet("get_difficulty/{id}")]
        public async Task<IActionResult> GetDifficulty(int id)
        {
            var request = new GetDificultyRequest
            {
                Id = id
            };
            var response = await _channelClient.GetDificultyAsync(request);
            if(response.NotFoundDifficulty != null)
            {
                if(response.NotFoundDifficulty.Code == 404)
                    return NotFound("Difficulty not found");
                return BadRequest(response.NotFoundDifficulty.Message);
            }
            return Ok(_mapper.Map<DifficultyDto>(response.DificultiesReply));
        }

        [HttpPost("add_difficulty")]
        public async Task<IActionResult> AddDifficulty([FromBody] DifficultyCreateDto difficulty)
        {
            if (!ModelState.IsValid)
            {
                return ReturnModelStateErrors();
            }
            return Ok();
        }
        private ActionResult ReturnModelStateErrors()
        {
            var errorList = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var errorString = string.Join(", ", errorList);

            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = errorString
            });
        }
    }
}
