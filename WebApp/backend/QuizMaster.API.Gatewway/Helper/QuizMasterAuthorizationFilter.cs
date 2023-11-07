using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuizMaster.API.Authentication.Helper;
using QuizMaster.API.Authentication.Services.Auth;

namespace QuizMaster.API.Gateway.Helper
{
    public class QuizMasterAuthorizationFilter: IAuthorizationFilter
    {
        private readonly IAuthenticationServices _authenticationServices;
        public QuizMasterAuthorizationFilter(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Retrieve the user's principal from the HttpContext
            var principal = context.HttpContext.User;

            if(principal.Identity == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check if the user is authenticated
            if (!principal.Identity.IsAuthenticated)
            {
                // If not authenticated, try to check if there is a JWT token in the header
                bool isJWTAuthenticated = IsJWTAuthenticated(context.HttpContext);
                if (!isJWTAuthenticated)
                {
                    // if still not authorized, return unauthorized
                    context.Result = new UnauthorizedResult();
                }
                return;
            }

            // Check the cookie for your token claim
            var tokenClaim = principal.Claims.FirstOrDefault(c => c.Type == "token")?.Value;

            if (string.IsNullOrEmpty(tokenClaim))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Validate the token as needed
            if (!IsTokenValid(tokenClaim, context.HttpContext))
            {
                context.Result = new UnauthorizedResult();
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
        private bool IsJWTAuthenticated(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers.Authorization.ToString().Split(" ")[1];

                // validate the token
                var authStore = _authenticationServices.Validate(token);

                // if auth store is null, the token specified failed to decode
                if (authStore == null)
                    return false;

                return true;
            }
            catch
            {
                // If error on splitting the token, return false
                return false;
            }
        }
    }
}
