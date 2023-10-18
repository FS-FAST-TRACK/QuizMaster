using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace QuizMaster.API.Authentication.Helper
{
    public class CookieFragment
    {
        public AuthenticationProperties AuthenticationProperties { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public CookieFragment(AuthenticationProperties AuthenticationProperties, ClaimsPrincipal claimsPrincipal)
        {
            this.AuthenticationProperties = AuthenticationProperties;
            ClaimsPrincipal = claimsPrincipal;
        }
    }
}
