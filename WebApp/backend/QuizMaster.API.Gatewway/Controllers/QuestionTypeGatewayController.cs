using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("api/gateway/question/type")]
    public class QuestionTypeGatewayController : Controller
    {
        private readonly IMapper _mapper;
        private readonly GrpcChannel _channel;
        private readonly QuizTypeService.QuizTypeServiceClient _channelClient;

        public QuestionTypeGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service);
            _channelClient = new QuizTypeService.QuizTypeServiceClient(_channel);
        }

        [HttpGet("get_all_type")]
        public async Task<IActionResult> GetAllType()
        {
            var request = new EmptyTypeRequest();
            var response = _channelClient.GetAllTypes(request);
            var types = new List<TypeDto>();
            while (await response.ResponseStream.MoveNext())
            {
                types.Add(_mapper.Map<TypeDto>(response.ResponseStream.Current));
            }
            return Ok(types);
        }

        [HttpGet("get_type/{id}")]
        public async Task<IActionResult> GetType(int id)
        {
            var request = new GetQuizTypeRequest{Id = id};
            var response = await _channelClient.GetQuizTypeAsync(request);
            if(response.Code == 404)
            {
                return NotFound(new { Type = "Error", Message = $"Type with id {id} not found." });
            }

            return Ok(_mapper.Map<TypeDto>(response.Type));
        }

        [HttpPost("add_type")]
        public async Task<IActionResult> AddType([FromBody] TypeCreateDto type)
        {
            var request = new AddQuisTypeRequest { QTypeDesc = type.QTypeDesc, QDetailRequired = type.QDetailRequired };
            var response = await _channelClient.AddQuizTypeAsync(request);

            if(response.Code == 409)
            {
                return Conflict(new { Type = "Error", Message = $"Type with description {type.QTypeDesc} already exists." });
            }

            if(response.Code == 500)
            {
                return BadRequest(new { Type = "Error", Message = $"Something went wrong." });
            }
            return Ok(new {id = response.Type.Id, qTypeDesc=response.Type.QTypeDesc, qDetailRequired = response.Type.QDetailRequired});
        }

        [HttpDelete("delete_type/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            var request = new GetQuizTypeRequest { Id = id };
            var response = await _channelClient.DeleteTypeAsync(request);

            if (response.Code == 404)
            {
                return ReturnTypeDoesNotExist(id);
            }

            if(response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update type." });
            }

            return NoContent();
        }

        [HttpPatch("update_type/{id}")]
        public async Task<IActionResult> UpdateType(int id, JsonPatchDocument<TypeCreateDto> patch)
        {
            var request = new UpdateTypeRequest();
            request.Id = id;
            request.Patch = JsonConvert.SerializeObject(patch);

            var response = await _channelClient.UpdateTypeAsync(request);

            if (response.Code == 404)
            {
                return ReturnTypeDoesNotExist(id);
            }

            if (response.Code == 409)
            {
                return Conflict(new { Type = "Error", Message = $"Type already exists." });
            }

            if (response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update type." });
            }

            return Ok(new { id = response.Type.Id, qTypeDesc = response.Type.QTypeDesc, qDetailRequired = response.Type.QDetailRequired });
        }

        private ActionResult ReturnTypeDoesNotExist(int id)
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = $"Type with id {id} doesn't exist."
            });

        }
    }
}
