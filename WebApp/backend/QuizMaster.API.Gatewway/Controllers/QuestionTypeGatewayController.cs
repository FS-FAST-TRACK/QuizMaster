using AutoMapper;
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
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;

        public QuestionTypeGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuizTypeService.QuizTypeServiceClient(_channel);
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);

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

            var request = new AddQuisTypeRequest { QTypeDesc = type.QTypeDesc, QDetailRequired = type.QDetailRequired };
            var response = await _channelClient.AddQuizTypeAsync(request, headers);

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

            var request = new GetQuizTypeRequest { Id = id };
            var response = await _channelClient.DeleteTypeAsync(request, headers);

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

            var request = new UpdateTypeRequest();
            request.Id = id;
            request.Patch = JsonConvert.SerializeObject(patch);

            var response = await _channelClient.UpdateTypeAsync(request, headers);

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
    }
}
