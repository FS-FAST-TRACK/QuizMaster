using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.Gateway.Services;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Threading.Channels;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/api/set")]
    public class QuizSetGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly QuizSetService.QuizSetServiceClient _channelClient;
        private readonly QuizRoomService.QuizRoomServiceClient roomChannelClient;
        private SessionHandler SessionHandler;
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;

        public QuizSetGatewayController(IOptions<GrpcServerConfiguration> options, SessionHandler sessionHandler)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _channel = GrpcChannel.ForAddress(options.Value.Session_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuizSetService.QuizSetServiceClient(_channel);
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service, new GrpcChannelOptions { HttpHandler = handler });
            roomChannelClient = new QuizRoomService.QuizRoomServiceClient(_channel);
            SessionHandler = sessionHandler;
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSet([FromBody] SetDTO setDTO)
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

            var request = new QuizSetRequest { QuizSet = JsonConvert.SerializeObject(setDTO) }; 
            var response = await _channelClient.AddQuizSetAsync(request, headers);

            if(response.Code == 404)
            { return NotFound(response.Message); }

            if (response.Code == 409)
            { return Conflict(response.Message); }

            if (response.Code == 500)
            { return BadRequest(response.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>( response.Data));
        }

        [HttpPost("submitAnswer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDTO answerDTO)
        {
            string Message = await SessionHandler.SubmitAnswer(roomChannelClient, answerDTO.ConnectionId, answerDTO.QuestionId, answerDTO.Answer);
            return Ok(new {  Message });
        }

        [HttpGet("all_set")]
        public async Task<IActionResult> GetAllSet()
        {
            var response = await _channelClient.GetAllQuizSetAsync(new QuizSetEmpty());
            
            if(response.Code == 500)
            {
                return BadRequest(response.Message);
            }

            return Ok(JsonConvert.DeserializeObject<Set[]>(response.Data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSet(int id)
        {
            var request = new GetQuizSetRequest { Id = id };
            var reply = await _channelClient.GetQuizSetAsync(request);

            if(reply.Code == 404)
            { return NotFound(reply.Message);}

            if(reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>(reply.Data));
        }

        [HttpGet("all_question_set")]
        public async Task<IActionResult> GetAllQuestionSet()
        {
            var response = await _channelClient.GetAllQuestionSetAsync(new QuizSetEmpty());

            if(response.Code == 500)
            { return BadRequest(response.Message); }

            return Ok(JsonConvert.DeserializeObject<QuestionSet[]>(response.Data));
        }

        [HttpGet("get_question_set/{id}")]
        public async Task<IActionResult> GetQuestionSet(int id)
        {
            var request = new GetQuizSetRequest { Id = id };
            var reply  = await _channelClient.GetQuestionSetAsync(request);

            if (reply.Code == 404)
            { return NotFound(reply.Message); }

            if (reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(JsonConvert.DeserializeObject<QuestionSet[]>(reply.Data));
        }

        [HttpPatch("update_set/{id}")]
        public async Task<IActionResult> UpdateSet(int id, [FromBody] SetDTO setDTO)
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

            var request = new QuizSetRequest { Id = id, QuizSet = JsonConvert.SerializeObject(setDTO) };
            var reply = await _channelClient.UpdateQuizSetAsync(request, headers);

            if(reply.Code == 404)
            { return NotFound(reply.Message); }

            if(reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>(reply.Data));
        }

        [HttpDelete("delete_set/{id}")]
        public async Task<IActionResult> DeleteSet(int id)
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

            var request = new GetQuizSetRequest { Id = id };
            var reply = await _channelClient.DeleteQuizSetAsync(request, headers);

            if (reply.Code == 404)
            { return NotFound(reply.Message); }

            if (reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(reply.Message);
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
