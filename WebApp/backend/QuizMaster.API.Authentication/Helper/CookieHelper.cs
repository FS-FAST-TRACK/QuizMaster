using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuizMaster.Library.Common.Utilities;
using System.Security.Claims;

namespace QuizMaster.API.Authentication.Helper
{
    public class CookieHelper
    {
        public static async Task ValidateCookie(CookieValidatePrincipalContext context ,string SecretKey)
        {
            var claimsPrincipal = context.Principal;

            if(claimsPrincipal != null)
            {
                // get the token
                var claimToken = claimsPrincipal.Claims.First(c => c.Type == "token").Value;

                if(claimToken != null)
                {
                    IDictionary<string, string> tokenData = JWTHelper.DecodeJsonWebToken(SecretKey, claimToken);


                    // temporary checks * reject cookie if decoded dictionary has no entries *
                    if(tokenData.Count == 0)
                    {
                        context.RejectPrincipal();
                        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
        }

        public static CookieFragment BuildCookie(string token)
        {
            // creating the claims
            var userClaims = new List<Claim>()
            {
                new Claim("token", token)
            };
            // creating the identity
            var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            // creating the principal
            var principal = new ClaimsPrincipal(identity);

            // settings for the auth properties
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = false,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            return new(authProperties, principal);
        }
    }
}
