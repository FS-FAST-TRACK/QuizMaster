using AutoMapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Threading.Channels;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/api/question_set")]
    public class QuizSetGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly QuizSetService.QuizSetServiceClient _channelClient;

        public QuizSetGatewayController(IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service);
            _channelClient = new QuizSetService.QuizSetServiceClient(_channel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSet([FromBody] SetDTO setDTO)
        {
            var request = new QuizSetRequest { QuizSet = JsonConvert.SerializeObject(setDTO) }; 
            var response = await _channelClient.AddQuizSetAsync(request);

            if(response.Code == 404)
            { return NotFound(response.Message); }

            if(response.Code == 500)
            { return BadRequest(response.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>( response.Data));
        }

        [HttpGet("all_question_set")]
        public async Task<IActionResult> GetAllSet()
        {
            var response = await _channelClient.GetAllQuizSetAsync(new QuizSetEmpty());
            
            if(response.Code == 500)
            {
                return BadRequest(response.Message);
            }

            return Ok(JsonConvert.DeserializeObject<QuestionSet[]>(response.Data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSet(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("update_set/{id}")]
        public async Task<IActionResult> UpdateSet(int id, [FromBody] SetDTO setDTO)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("delete_set/{id}")]
        public async Task<IActionResult> DeleteSet(int id)
        {
            throw new NotImplementedException();
        }
    }
}
