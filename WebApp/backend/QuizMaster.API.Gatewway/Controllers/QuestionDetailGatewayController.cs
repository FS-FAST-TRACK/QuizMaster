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
    [Route("api/gateway/question/{questionId}/question_detail")]
    public class QuestionDetailGatewayController : Controller
    {
        private readonly IMapper _mapper;
        private readonly GrpcChannel _channel;
        private readonly QuestionDetailServices.QuestionDetailServicesClient _channelClient;
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;

        public QuestionDetailGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuestionDetailServices.QuestionDetailServicesClient(_channel);
            _mapper = mapper;
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
        }

        [HttpGet("get_question_details")]
        public async Task<ActionResult<IEnumerable<QuestionDetailDto>>> GetQuestionDetails(int questionId)
        {
            // Process Input
            var request = new QuestionDetailRequest
            {
                QuestionId = questionId
            };

            // Process Logic
            var response = await _channelClient.GetQuestionDetailsAsync(request);


           

            // Process Output
            if (response.Code == 404)
            {
                return NotFound(new ResponseDto { Type = "Error", Message = response.Content });
            }

            try
            {
                var questionDetails = JsonConvert.DeserializeObject<IEnumerable<QuestionDetail>>(response.Content);
              

                return Ok(_mapper.Map<IEnumerable<QuestionDetailDto>>(questionDetails));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("get_question_detail/{id}")] 
        public async Task<IActionResult> GetQuestionDetail(int questionId, int id)
        {
            // Process Input
            var request = new QuestionDetailRequest() { Id = id, QuestionId = questionId};

            // Process logic
            var response = await _channelClient.GetQuestionDetailAsync(request);

            // Process output
            if(response.Code == 404)
            {
                return NotFound(new ResponseDto { Type = "Error", Message = response.Content });
            }
             
            var questionDetail = JsonConvert.DeserializeObject<QuestionDetail>(response.Content);
            return Ok(_mapper.Map<QuestionDetailDto>(questionDetail));
        }

        [HttpPost("add_question_detail")]
        public async Task<IActionResult> AddQuestionDetail(int questionId, [FromBody] QuestionDetailCreateDto questionDetailDto)
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


            // Process Input
            var questionDetail = JsonConvert.SerializeObject(questionDetailDto);
            var request = new QuestionDetailRequest() { QuestionId = questionId, Content = questionDetail };

            // Process Logic
            var reply = await _channelClient.AddQuestionDetailAsync(request, headers);


            // Process Output
            if (reply.Code == 409)
            {
                return ReturnQuestionDetailAlreadyExist();
            }

            if(reply.Code == 404)
            {
                return StatusCode(StatusCodes.Status404NotFound, reply.Content);
            }

            if(reply.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create question detail." });
            }

            var createdQuestionDetail = JsonConvert.DeserializeObject<QuestionDetailDto>(reply.Content);
            return Ok(createdQuestionDetail);
        }

        [HttpDelete("delete_question_detail/{id}")]
        public async Task<IActionResult> DeleteQuestionDetail(int questionId, int id)
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

            var request = new QuestionDetailRequest() { QuestionId= questionId, Id = id };
            var reply = await _channelClient.DeleteQuestionDetailAsync(request, headers);

            if(reply.Code == 404)
            {
                return StatusCode(StatusCodes.Status404NotFound, reply.Content);
            }

            if(reply.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete question." });
            }

            return NoContent();
        }

        [HttpPatch("update_question_detail/{id}")]
        public async Task<IActionResult> UpdateQuestionDetail(int questionId, int id,[FromBody] JsonPatchDocument<QuestionDetailCreateDto> patch)
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
            // Process Input
            var patchRequest = JsonConvert.SerializeObject(patch);
            var request = new QuestionDetailRequest() {QuestionId=questionId,  Id = id, Content = patchRequest };

            // Process Logic
            var reply = await _channelClient.PatchQuestionDetailAsync(request, headers);
            
            // Process Output
            if(reply.Code == 404)
            {
                return StatusCode(StatusCodes.Status404NotFound, reply.Content);
            }

            if (reply.Code == 500)
            {
                return BadRequest(new ResponseDto
                {
                    Type = "Error",
                    Message = reply.Content
                });
            }
            
            if(reply.Code == 409)
            {
                return ReturnQuestionDetailAlreadyExist();
            }

            if (reply.Code == 400)
            {
                return BadRequest(new ResponseDto
                {
                    Type = "Error",
                    Message = reply.Content
                }) ;
            }

            var result = JsonConvert.DeserializeObject<QuestionDetail>(reply.Content);
            return Ok(_mapper.Map<QuestionDetailDto>(result));
        }

        #region Utility
        private ActionResult ReturnQuestionDetailDoesNotExist(int id)
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = $"QuestionDetail with id {id} doesn't exist."
            });

        }

        private ActionResult ReturnQuestionDetailAlreadyExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "QuestionDetail already exist."
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
