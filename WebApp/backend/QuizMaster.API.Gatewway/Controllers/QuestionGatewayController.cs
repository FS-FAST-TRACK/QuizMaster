using AutoMapper;
using Azure;
using Grpc.Net.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers.Quiz;
using QuizMaster.Library.Common.Models;
using System.Threading.Channels;

namespace QuizMaster.API.Gateway.Controllers
{
    [Route("api/gateway/question")]
    public class QuestionGatewayController : Controller
    {
        private readonly IMapper _mapper;
        private readonly GrpcChannel _channel;
        private readonly QuestionServices.QuestionServicesClient _channelClient;

        public QuestionGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service);
            _channelClient = new QuestionServices.QuestionServicesClient(_channel);
            _mapper = mapper;
        }

        [HttpGet("get_questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions([FromQuery] QuestionResourceParameter resourceParameter)
        {
            var request = new QuestionRequest
            {
                Parameter = JsonConvert.SerializeObject(resourceParameter)
            };

            var response = await _channelClient.GetQuestionsAsync(request);

            var questions = JsonConvert.DeserializeObject<PagedList<Question>>(response.Questions);

            var paginationMetadata = new Dictionary<string, object?>
                {
                    { "totalCount", questions.TotalCount },
                    { "pageSize", questions.PageSize },
                    { "currentPage", questions.CurrentPage },
                    { "totalPages", questions.TotalPages },
                    { "previousPageLink", questions.HasPrevious ?
                        Url.Link("GetQuestions", resourceParameter.GetObject("prev"))
                        : null },
                    { "nextPageLink", questions.HasNext ?
                        Url.Link("GetQuestions", resourceParameter.GetObject("next"))
                        : null }
                };

            Response.Headers.Add("X-Pagination",
                   System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
        }

        [HttpGet("get_question/{id}")] 
        public async Task<IActionResult> GetQuestion(int id)
        {
            var request = new GetQuestionRequest() { Id = id};

            var response = await _channelClient.GetQuestionAsync(request);

            if(response.Code == 404)
            {
                return NotFound(new ResponseDto { Type = "Error", Message = $"Question with id {id} not found." });
            }
            var question = JsonConvert.DeserializeObject<QuestionDto>(response.Questions);
            return Ok(question);
        }

        [HttpPost("add_question")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionCreateDto questionDto)
        {
            if(!ModelState.IsValid)

            {
                return ReturnModelStateErrors();
            }
            var validationResult = questionDto.IsValid();
            if (!validationResult.IsValid)
            {
                return BadRequest(
                    new ResponseDto { Type = "Error", Message = validationResult.Error }
                );
            }

            var question = JsonConvert.SerializeObject(questionDto);
            var request = new QuestionRequest() { Parameter = question };

            var reply = await _channelClient.AddQuestionAsync(request);

            if (reply.Code == 409)
            {
                return ReturnQuestionAlreadyExist();
            }

            if(reply.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create question." });
            }

            var createdQuestion = JsonConvert.DeserializeObject<QuestionDto>(reply.Questions);
            return Ok(createdQuestion);
        }

        [HttpDelete("delete_question/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var request = new GetQuestionRequest() { Id = id };
            var reply = await _channelClient.DeleteQuestionAsync(request);

            if(reply.Code == 404)
            {
                return ReturnQuestionDoesNotExist(id);
            }

            if(reply.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete question." });
            }

            return NoContent();
        }

        [HttpPatch("update_question/{id}")]
        public async Task<IActionResult> UpdateQuestion(int id,[FromBody] JsonPatchDocument<QuestionCreateDto> patch)
        {
            var patchRequest = JsonConvert.SerializeObject(patch);
            var request = new PatchQuestionRequest() { Id = id, Patch = patchRequest };

            var reply = await _channelClient.PatchQuestionAsync(request);
            
            if(reply.Code == 404)
            {
                return ReturnQuestionDoesNotExist(id);
            }

            if (reply.Code == 500)
            {
                return BadRequest(new ResponseDto
                {
                    Type = "Error",
                    Message = reply.Questions
                });
            }
            
            if(reply.Code == 409)
            {
                return ReturnQuestionAlreadyExist();
            }

            if (reply.Code == 400)
            {
                return BadRequest(new ResponseDto
                {
                    Type = "Error",
                    Message = reply.Questions
                }) ;
            }

            var result = JsonConvert.DeserializeObject<Question>(reply.Questions);
            return Ok(_mapper.Map<QuestionDto>(result));
        }

        #region Utility
        private ActionResult ReturnQuestionDoesNotExist(int id)
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = $"Question with id {id} doesn't exist."
            });

        }

        private ActionResult ReturnQuestionAlreadyExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "Question already exist."
            });
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
        #endregion
    }
}
