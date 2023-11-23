using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Helper;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Gatewway.Controllers
{
    [ApiController]
    [Route("gateway/api")]
    public class AccountGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly AccountService.AccountServiceClient _channelClient;
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;

        private readonly IMapper _mapper;

        public AccountGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Account_Service);
            _channelClient = new AccountService.AccountServiceClient(_channel);
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service);
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
            _mapper = mapper;
        }

        /// <summary>
        /// Get account API by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpGet("account/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new GetAccountByIdRequest
            {
                Id = id
            };

            var response = await _channelClient.GetAccountByIdAsync(request);
            if (response.UserNotFound != null)
            {
                return NotFound(response.UserNotFound);
            }

            var account = JsonConvert.DeserializeObject<UserAccount>(response.GetAccountByIdReply.Account);

            return Ok(_mapper.Map<AccountDto>(account));
        }

        /// <summary>
        /// Get all account API
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAdminAuthorization]
        [HttpGet("account")]
        public async Task<IActionResult> GetAllUsers()
        {
            var request = new Empty();

            var response = _channelClient.GetAllUsers(request);

            var users = new List<AllUserReply>();
            while (await response.ResponseStream.MoveNext())
            {
                users.Add(response.ResponseStream.Current);
            }

            return Ok(users);
        }

        /// <summary>
        /// Create account
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost("account/create")]
        public async Task<IActionResult> Create(AccountCreateDto account)
        {
            if (!ModelState.IsValid)
            {
                return ReturnModelStateErrors();
            }

            var checkUsername = new CheckUserNameRequest
            {
                Username = account.UserName
            };

            var response = await _channelClient.CheckUserNameAsync(checkUsername);

            if (!response.IsAvailable)
            {
                return ReturnUserNameAlreadyExist();
            }
            var checkEmail = new CheckEmailRequest
            {
                Email = account.Email
            };

            var emailResponse = await _channelClient.CheckEmailAsync(checkEmail);

            if (!emailResponse.IsAvailable)
            {
                return ReturnEmailAlreadyExist();
            }

            var request = _mapper.Map<CreateAccountRequest>(account);

            var reply = await _channelClient.CreateAccountAsync(request);

            return Ok(reply);
        }

        [HttpPost("account/create_partial")]
        public async Task<IActionResult> CreatePartial(AccountCreatePartialDto account)
        {
            if (!ModelState.IsValid)
            {
                return ReturnModelStateErrors();
            }

            var checkUsername = new CheckUserNameRequest
            {
                Username = account.UserName
            };

            var response = await _channelClient.CheckUserNameAsync(checkUsername);

            if (!response.IsAvailable)
            {
                return ReturnUserNameAlreadyExist();
            }

            var checkEmail = new CheckEmailRequest
            {
                Email = account.Email
            };

            var emailResponse = await _channelClient.CheckEmailAsync(checkEmail);

            if (!emailResponse.IsAvailable)
            {
                return ReturnEmailAlreadyExist();
            }



            var request = _mapper.Map<CreateAccountPartialRquest>(account);
            var reply = await _channelClient.CreateAccountPartialAsync(request);

            return Ok(reply);
        }

        [QuizMasterAuthorization]
        [HttpDelete("account/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
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
            var request = new DeleteAccountRequest
            {
                Id = id
            };

            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };


            var reply = await _channelClient.DeleteAccountAsync(request, headers);

            if (reply.StatusCode == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete user." });
            }
            else if (reply.StatusCode == 404)
            {
                return NotFound("Account does not exist");
            }

            return NoContent();
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



        [QuizMasterAuthorization]
        [HttpPatch("account/update/{id}")]
        public async Task<IActionResult> Update(int id, JsonPatchDocument<UserAccount> patch)
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

            var request = new GetAccountByIdRequest
            {
                Id = id
            };
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var response = await _channelClient.GetAccountByIdAsync(request);
            if (response.UserNotFound != null)
            {
                return NotFound(response.UserNotFound);
            }

            var account = JsonConvert.DeserializeObject<UserAccount>(response.GetAccountByIdReply.Account);

            patch.ApplyTo(account);

            if (!ModelState.IsValid)
            {
                return ReturnModelStateErrors();
            }

            var update = new UpdateAccountRequest
            {
                Account = JsonConvert.SerializeObject(account)
            };


            var updateReply = await _channelClient.UpdateAccountAsync(update, headers);
            if (updateReply.StatusCode == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update user." });
            }

            return NoContent();
        }

        //[QuizMasterAdminAuthorization]
        [HttpPost]
        [Route("account/set_admin/{username}")]
        public async Task<IActionResult> SetAdmin(string username, [FromQuery] bool setAdmin = false)
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


            var request = new SetAccountAdminRequest
            {
                Username = username,
                SetAdmin = setAdmin
            };
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var response = await _channelClient.SetAdminAccountAsync(request, headers);

            if(response.Code == 404)
            {
                return NotFound(new {response.Message });
            }
            if(response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new { response.Message });
            }

            return Ok(new { response.Message });
        }

        // Check if model is valid
        private IActionResult ReturnModelStateErrors()
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

        // Return id username already exist
        private ActionResult ReturnUserNameAlreadyExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "UserName already exist."
            });
        }

        // Return if user doesn't exist
        private ActionResult ReturnUserDoesNotExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "User doesn't exist."
            });

        }

        // Return if email already exist
        private ActionResult ReturnEmailAlreadyExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "Email already exist."
            });
        }
    }
}