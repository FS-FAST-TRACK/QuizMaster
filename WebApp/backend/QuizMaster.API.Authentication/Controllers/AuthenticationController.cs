using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Authentication.Helper;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Services.Auth;

namespace QuizMaster.API.Authentication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServices _authenticationServices;

        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequestDTO requestModel)
        {
            // generate the token by calling the authentication service
            var tokenHolder = await _authenticationServices.Authenticate(requestModel.GetAuthRequest());

            // if no token is generated, it is an invalid credentials
            if(tokenHolder.Token == null) {
                // this will clear a cookie if there is any
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Unauthorized(new { Message = "Invalid Credentials" }); 
            };

            // create the cookie
            var cookieFragment = CookieHelper.BuildCookie(tokenHolder.Token);

            // sign in user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cookieFragment.ClaimsPrincipal, cookieFragment.AuthenticationProperties);

            return Ok(new { Message = "Logged in successfully", tokenHolder.Token });
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            // simply call the SightOutAsync method in the HttpContext object to sign out user.
            // this clear's the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new { Message = "Logged out successfully" });
        }

        [Authorize]
        [HttpGet]
        [Route("info")]
        public IActionResult GetCookieInfo()
        {
            // grab the claims identity
            var tokenClaim = User.Claims.ToList().FirstOrDefault(e => e.Type == "token");

            if(tokenClaim == null) { return NotFound(new { Message = "No information found based on session" }); }

            string token = tokenClaim.Value;

            // get the AuthStore based on token
            var authStore = _authenticationServices.Validate(token);

            if (authStore == null) return NotFound(new { Message = "No information found based on the token provided" });

            return Ok(new { Message = "Info", authStore });
        }

        //[Authorize]
        //[HttpPost]
        //[Route("set_admin/{Username}")]
        //public async Task<IActionResult> SetAdmin(string Username, [FromQuery] bool isAdmin = false)
        //{
        //    var response = await _authenticationServices.UpdateRole(new AuthRequest { Username = Username }, isAdmin);
        //    return Ok(response);
        //}
    }
}
