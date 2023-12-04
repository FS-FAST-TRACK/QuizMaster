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
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service);
            _channelClient = new QuizDifficultyService.QuizDifficultyServiceClient(_channel);
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service);
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
        }

        [HttpGet("get_all_difficulty")]
        public async Task<IActionResult> GetAllDifficulty()
        {
            var request = new EmptyDifficultyRequest();
            var response = _channelClient.GetDificulties(request);
            var difficulties = new List<DifficultyDto>();
            while (await response.ResponseStream.MoveNext())
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
            if (response.NotFoundDifficulty != null)
            {
                if (response.NotFoundDifficulty.Code == 404)
                    return NotFound("Difficulty not found");
                return BadRequest(response.NotFoundDifficulty.Message);
            }
            return Ok(_mapper.Map<DifficultyDto>(response.DificultiesReply));
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
            var request = new GetDifficultyByDescRequest
            {
                Desc = difficulty.QDifficultyDesc
            };
            if (!ModelState.IsValid)
            {
                return ReturnModelStateErrors();
            }

            var checkDifficulty = await _channelClient.GetDifficultyByDescAsync(request);

            if (checkDifficulty.Code == 200)
            {
                return ReturnDifficultyAlreadyExist();
            }

            if(checkDifficulty.Code == 201)
            {
                return Ok(new { id = checkDifficulty.Id, qDifficultyDesc = difficulty.QDifficultyDesc });
            }

            if (checkDifficulty.Code == 400)
            {
                var createDifficulty = await _channelClient.CreateDifficultyAsync(_mapper.Map<CreateDifficultyRequest>(difficulty), headers);
                if (createDifficulty.Code == 500)
                {
                    return BadRequest("Failed to create difficulty");
                }

                return Ok(new { id = createDifficulty.Id, qDifficultyDesc  = createDifficulty.QDifficultyDesc});
            }
            return BadRequest("Something went wrong");
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

            var request = new GetDificultyRequest() { Id = id };
            var resposnse = await _channelClient.DeleteDifficultyAsync(request, headers);
            if(resposnse.Code == 404)
            {
                return ReturnDifficultyDoesNotExist(id);
            }
            if(resposnse.Code == 500)
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

            var request = new UpdateDifficultyRequest();
            request.Id = id;
            request.Patch = JsonConvert.SerializeObject(patch);

            var response = await _channelClient.UpdateDifficultyAsync(request, headers);

            if(response.Code == 404)
            {
                return ReturnDifficultyDoesNotExist(id);
            }

            if(response.Code == 409)
            {
                return ReturnDifficultyAlreadyExist();
            }

            if(response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update difficulty." });
            }

            return Ok(new { id = response.Id, qDifficultyDesc = response.QDifficultyDesc });
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
