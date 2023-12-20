 using AutoMapper;
using Azure;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
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
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;

        public QuestionGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuestionServices.QuestionServicesClient(_channel);
            _mapper = mapper;
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
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
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };



            if (!ModelState.IsValid)

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

            var reply = await _channelClient.AddQuestionAsync(request, headers);

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
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var request = new GetQuestionRequest() { Id = id };
            var reply = await _channelClient.DeleteQuestionAsync(request, headers);

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
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var patchRequest = JsonConvert.SerializeObject(patch);
            var request = new PatchQuestionRequest() { Id = id, Patch = patchRequest };

            var reply = await _channelClient.PatchQuestionAsync(request, headers);
            
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
        private async Task<AuthStore> ValidateUserTokenAndGetInfo()
        {
            string token = GetUserToken();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var info = await GetAuthStoreInfo(token);

            if (info == null || info.UserData == null)
            {
                return null;
            }

            return info;
        }

        private string GetUserToken()
        {
            var tokenClaim = User.Claims.FirstOrDefault(e => e.Type == "token");

            if (tokenClaim != null)
            {
                return tokenClaim.Value;
            }

            try
            {
                return HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
            }
            catch
            {
                return null;
            }
        }

        private async Task<AuthStore> GetAuthStoreInfo(string token)
        {
            var requestValidation = new ValidationRequest()
            {
                Token = token
            };

            var authStore = await _authChannelClient.ValidateAuthenticationAsync(requestValidation);

            return !string.IsNullOrEmpty(authStore?.AuthStore) ? JsonConvert.DeserializeObject<AuthStore>(authStore.AuthStore) : null;
        }
        #endregion
    }
}
