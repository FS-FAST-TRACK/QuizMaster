using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Configuration;
using QuizMaster.API.Authentication.Services.Auth;
using QuizMaster.Library.Common.Utilities;

namespace QuizMaster.API.Gateway.Filters
{
    public class QuizMasterAdminAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly ILogger<QuizMasterAdminAuthorizationFilter> _logger;
        private readonly AppSettings _appSettings;
        public QuizMasterAdminAuthorizationFilter(IAuthenticationServices authenticationServices, ILogger<QuizMasterAdminAuthorizationFilter> logger, IOptions<AppSettings> options)
        {
            _authenticationServices = authenticationServices;
            _logger = logger;
            _appSettings = options.Value;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Retrieve the user's principal from the HttpContext
            var principal = context.HttpContext.User;
            string authType = context.HttpContext.Request.Headers.Authorization.ToString().Split(" ").Length > 1 ? "Token Based" : "Cookie Based";

            if (principal.Identity == null)
            {
                context.Result = new UnauthorizedObjectResult(new { Status = 401, Message = "Unauthorized, please login", AuthType = authType });
                return;
            }

            // Check if the user is authenticated
            if (!principal.Identity.IsAuthenticated)
            {
                // If not authenticated, try to check if there is a JWT token in the header
                var (isJWTAuthenticated, Message, Status) = IsJWTAuthenticated(context.HttpContext);
                if (!isJWTAuthenticated)
                {
                    var result = new ObjectResult(new { Status, Message, AuthType = authType });
                    result.StatusCode = Status;
                    context.Result = result;
                }
                return;
            }

            // Check the cookie for your token claim
            var tokenClaim = principal.Claims.FirstOrDefault(c => c.Type == "token")?.Value;

            if (string.IsNullOrEmpty(tokenClaim))
            {
                context.Result = new UnauthorizedObjectResult(new { Status = 401, Message = "Unauthorized, please login", AuthType = authType });
                return;
            }

            // Validate the token as needed
            if (!IsTokenValid(tokenClaim, context.HttpContext))
            {
                context.Result = new UnauthorizedObjectResult(new { Status = 401, Message = "Unauthorized, token expired", AuthType = authType });
            }

            // Ensure Admin
            if (!IsAdmin(tokenClaim, context.HttpContext))
            {
                var result = new ObjectResult(new { Status = 403, Message = "Forbidden, Administrator role required", AuthType = authType });
                result.StatusCode = 403;
                context.Result = result;
            }

            if (!context.HttpContext.Items.ContainsKey("token"))
            {
                context.HttpContext.Items["token"] = tokenClaim;
            }
        }

        private bool IsTokenValid(string token, HttpContext httpContext)
        {
            // validate the token
            var authStore = _authenticationServices.Validate(token);

            // if auth store is null, the token specified failed to decode
            if (authStore == null)
                return false;

            // if the token expired, return false
            if (authStore.IsExpired())
                return false;

            // If token is valid, return true; otherwise, return false.
            return true;
        }

        private bool IsAdmin(string token, HttpContext httpContext)
        {
            // validate the token
            var authStore = _authenticationServices.Validate(token);

            // if auth store is null, the token specified failed to decode
            if (authStore == null)
                return false;

            // if the role is not admin, return false
            var userRole = authStore.Roles.FirstOrDefault(r => r == "Administrator");
            if (userRole == null)
                return false;

            return true;
        }

        private (bool, string, int) IsJWTAuthenticated(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers.Authorization.ToString().Split(" ")[1];

                // validate the token
                var authStore = _authenticationServices.Validate(token);

                // if auth store is null, the token specified failed to decode
                if (authStore == null)
                    return (false, "Unauthorized,Invalid Token", 401);

                // if the role is not admin, return false
                var userRole = authStore.Roles.FirstOrDefault(r => r == "Administrator");
                if (userRole == null)
                    return (false, "Unauthorized, Administrator role required", 403);

                context.Items["token"] = token;

                return (!authStore.IsExpired(), authStore.IsExpired() ? "Unauthorized, Token expired" : "", 401);
            }
            catch
            {
                // If error on splitting the token, return false
                return (false, "Unauthorized, please login", 401);
            }
        }
    }
}
