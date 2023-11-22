using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Authentication.Models;
using Grpc.Core;
using Grpc.Net.Client;
using QuizMaster.API.Authentication.Proto;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using QuizMaster.API.Authentication.Helper;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Helper;
using Microsoft.Extensions.Options;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/api/auth")]
    public class AuthenticationGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly AuthService.AuthServiceClient _channelClient;

        public AuthenticationGatewayController(IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Authentication_Service);
            _channelClient = new AuthService.AuthServiceClient(_channel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequestDTO requestModel)
        {
            var request = new AuthenticationRequest()
            {
                Username = requestModel.Username,
                Email = requestModel.Email,
                Password = requestModel.Password,

            };

            // generate the token by calling the authentication service
            var reply = await _channelClient.GetAuthenticationAsync(request);

            // if no token is generated, it is an invalid credentials
            if (reply.Token == "")
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Unauthorized(new { Message = "Invalid Credentials" });
            };

            // create the cookie
            var cookieFragment = CookieHelper.BuildCookie(reply.Token);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cookieFragment.ClaimsPrincipal, cookieFragment.AuthenticationProperties);

            return Ok(new { Message = "Logged in successfully", reply.Token });
        }

        [QuizMasterAuthorization]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            // simply call the SightOutAsync method in the HttpContext object to sign out user.
            // this clear's the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new { Message = "Logged out successfully" });
        }

        [QuizMasterAuthorization]
        [HttpGet("info")]
        public async Task<IActionResult> GetCookieInfo()
        {
            string token = string.Empty;
            // grab the claims identity
            var tokenClaim = User.Claims.ToList().FirstOrDefault(e => e.Type == "token");

            if(tokenClaim == null) 
            {
                // Check the request header if there is a JWT token
                try
                {
                    token = HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
                }
                catch
                {
                    return NotFound(new { Message = "No information found based on session" });
                }
            }
            else
            {
                token = tokenClaim.Value;
            }
            

            var request = new ValidationRequest()
            {
                Token = token
            };

            // get the AuthStore based on token
            var authStore = await _channelClient.ValidateAuthenticationAsync(request);

            if (authStore.AuthStore == "") return NotFound(new { Message = "No information found based on the token provided" });

            var info = JsonConvert.DeserializeObject<AuthStore>(authStore.AuthStore);

            return Ok(new { Message = "Info", info });
        }

        [QuizMasterAdminAuthorization]
        [HttpPost]
        [Route("set_admin/{username}")]
        public async Task<IActionResult> SetAdmin(string username)
        {
            var request = new SetAdminRequest()
            {
                Username = username
            };

            var response = await _channelClient.SetAdminAsync(request);
            var info = JsonConvert.DeserializeObject<ResponseDto>(response.Response);
            return Ok(info);
        }
    }
}
