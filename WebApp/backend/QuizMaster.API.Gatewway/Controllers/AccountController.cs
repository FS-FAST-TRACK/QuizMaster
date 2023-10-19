using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Account.Proto;

namespace QuizMaster.API.Gatewway.Controllers
{
    public class AccountController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly AccountService.AccountServiceClient _channelClient;

        public AccountController()
        {
            _channel = GrpcChannel.ForAddress("https://localhost:7175");
            _channelClient = new AccountService.AccountServiceClient(_channel);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
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
    }
}
