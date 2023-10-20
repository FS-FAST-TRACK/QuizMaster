using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Gatewway.Controllers
{
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
        [HttpGet]
        [Route("api/Gateway/[controller]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = new RegisterRequest
            {
                Id = id
            };

            var response = await _channelClient.RegisterAsync(request);
            if(response.UserNotFound != null)
            {
                return NotFound(response.UserNotFound);
            }

            return Ok(response.RegisterResponse);
        }

        /// <summary>
        /// Get all account API
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        [Route("api/Gateway/[controller]/GetAllUsers")]
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
        [HttpPost]
        [Route("api/Gateway/[controller]/CreateAccount")]
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

        [HttpPost]
        [Route("gateway/api/[controller]/create_account_patrial")]
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
    }
}
