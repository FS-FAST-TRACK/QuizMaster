using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models;
using System.Globalization;

namespace QuizMaster.API.Gatewway.Controllers
{
    [ApiController]
    [Route("gateway/api")]
    public class AccountGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly AccountService.AccountServiceClient _channelClient;
        private readonly IMapper _mapper;

        public AccountGatewayController(IMapper mapper)
        {
            _channel = GrpcChannel.ForAddress("https://localhost:7175");
            _channelClient = new AccountService.AccountServiceClient(_channel);
            _mapper = mapper;
        }

        /// <summary>
        /// Get account API by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet("get_account/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new GetAccountByIdRequest
            {
                Id = id
            };

            var response = await _channelClient.GetAccountByIdAsync(request);
            if(response.UserNotFound != null)
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
        [HttpGet("get_all_users")]
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
        [HttpPost("create_account")]
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

            var request = _mapper.Map<CreateAccountRequest>(account);

            var reply = await _channelClient.CreateAccountAsync(request);

            return Ok(reply);
        }

        [HttpPost("partial_create_account")]
        public async Task<IActionResult> CreatePartial(AccountCreatePartialDto account)
        { 
            if(!ModelState.IsValid)
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

            var request = _mapper.Map<CreateAccountPartialRquest>(account);
            var reply = await _channelClient.CreateAccountPartialAsync(request);

            return Ok(reply);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new DeleteAccountRequest
            {
                Id = id
            };

            var reply = await _channelClient.DeleteAccountAsync(request);

            if(reply.StatusCode == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete user." });
            }
            else if(reply.StatusCode == 404)
            {
                return NotFound("Account does not exists");
            }

            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update(int id, JsonPatchDocument<UserAccount> patch)
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

            patch.ApplyTo(account);

            if (!ModelState.IsValid)
            {
                return ReturnModelStateErrors();
            }

            var update = new UpdateAccountRequest
            {
                Account = JsonConvert.SerializeObject(account)
            };


            var updateReply = await _channelClient.UpdateAccountAsync(update);
            if (updateReply.StatusCode == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update user." });
            }

            return NoContent();
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
    }
}
