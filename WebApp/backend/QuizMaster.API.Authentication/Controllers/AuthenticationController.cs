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
        public async Task<IActionResult> Login([FromBody] AuthRequest requestModel)
        {
            // generate the token by calling the authentication service
            var tokenHolder = _authenticationServices.Authenticate(requestModel);

            // if no token is generated, it is an invalid credentials
            if(tokenHolder.Token == null) { return Unauthorized(new { Message = "Invalid Credentials" }); };

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
        [HttpPost]
        [Route("set_admin/{id}")]
        public IActionResult SetAdmin(int id)
        {
            return Ok();
        }
    }
}
