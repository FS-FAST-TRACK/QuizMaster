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
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.Library.Common.Entities.Questionnaire;
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
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;
        public QuestionDifficultyGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuizDifficultyService.QuizDifficultyServiceClient(_channel);
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
        }

        [HttpGet("get_difficulties")]
        public async Task<IActionResult> GetPagedDifficulties([FromQuery] DifficultyResourceParameter resourceParameter)
        {
            // Proccess Input
            var request = new DifficultyRequest()
            {
                Content = JsonConvert.SerializeObject(resourceParameter),
                Type = "difficultyResourceParameter",
            };


            // Process Logic
            if (resourceParameter.IsGetAll)
            {
                var response = await _channelClient.GetAllDifficultyAsync(request);
                var difficulties = JsonConvert.DeserializeObject<IEnumerable<QuestionDifficulty>>(response.Content);

                return Ok(_mapper.Map<IEnumerable<DifficultyDto>>(difficulties));
            }

            var pagedResponse = await _channelClient.GetDifficultiesAsync(request);

            // Process output
           
            try
            {
                var pagedDifficulties = JsonConvert.DeserializeObject<Tuple<IEnumerable<DifficultyDto>, Dictionary<string, object?>>>(pagedResponse.Content);
            Response.Headers.Add("X-Pagination",
                   JsonConvert.SerializeObject(pagedDifficulties!.Item2));

            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            return Ok(pagedDifficulties.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }


        }


        [HttpGet("get_difficulty/{id}")]
        public async Task<IActionResult> GetDifficulty(int id)
        {

            // Process Input
            var request = new DifficultyRequest
            {
                Content = JsonConvert.SerializeObject(id),
                Type = "int",
            };

            // Process Logic
            var response = await _channelClient.GetDifficultyAsync(request);

            // Process Output
            if (response.Code == 404)
            {

                return NotFound("Difficulty not found");

            }
            return Ok(_mapper.Map<DifficultyDto>(JsonConvert.DeserializeObject<QuestionDifficulty>(response.Content)));
        }

        [HttpPost("add_difficulty")]
        public async Task<IActionResult> AddDifficulty([FromBody] DifficultyCreateDto difficulty)
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
            var request = new DifficultyRequest
            {
                Content = JsonConvert.SerializeObject(difficulty),
                Type = "difficultyCreateDto"
            };

            // Process Logic
            var response = await _channelClient.CreateDifficultyAsync(request, headers);

            // Process Output
            if (response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Content);
            }
            var createDifficulty = JsonConvert.DeserializeObject<QuestionDifficulty>(response.Content);
            
            return Ok(_mapper.Map<DifficultyDto>(createDifficulty));

        }

        [HttpDelete("delete_difficulty/{id}")]
        public async Task<IActionResult> DeleteDifficulty(int id)
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
            var request = new DifficultyRequest() { Content = JsonConvert.SerializeObject(id) };

            // Process Logic
            var resposnse = await _channelClient.DeleteDifficultyAsync(request, headers);

            // Process Output
            if (resposnse.Code == 404)
            {
                return ReturnDifficultyDoesNotExist(id);
            }
            if (resposnse.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update difficulty." });
            }
            return NoContent();
        }

        [HttpPatch("update_difficulty/{id}")]
        public async Task<IActionResult> UpdateDifficulty(int id, JsonPatchDocument<DifficultyCreateDto> patch)
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
            var request = new DifficultyRequest()
            {
                Content = JsonConvert.SerializeObject(new Tuple<int, JsonPatchDocument<DifficultyCreateDto>>(id, patch))
            };

            // Process Logic
            var response = await _channelClient.UpdateDifficultyAsync(request, headers);

            // Process Output
            if (response.Code == 404)
            {
                return ReturnDifficultyDoesNotExist(id);
            }

            if (response.Code == 409)
            {
                return ReturnDifficultyAlreadyExist();
            }

            if (response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update difficulty." });
            }

            var createDifficulty = JsonConvert.DeserializeObject<QuestionDifficulty>(response.Content);

            return Ok(_mapper.Map<DifficultyDto>(createDifficulty));
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
        private ActionResult ReturnDifficultyAlreadyExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "Difficulty already exist."
            });
        }
        private ActionResult ReturnDifficultyDoesNotExist(int id)
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = $"Difficulty with id {id} doesn't exist."
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
