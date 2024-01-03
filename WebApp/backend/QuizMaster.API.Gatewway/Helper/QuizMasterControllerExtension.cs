using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.API.Gateway.Helper
{
    public static class QuizMasterControllerExtension
    {
        public static string? GetToken(this Controller controller)
        {
            string? token = null;

            // get token if token based authentication
            string[] authHeader = controller.HttpContext.Request.Headers.Authorization.ToString().Split(" ");
            if(authHeader.Length > 1)
            {
                token = authHeader[^1];
            }

            // try to get the token if cookie based authentication
            if(string.IsNullOrEmpty(token) )
            {
                var user = controller.HttpContext.User;
                if (user.Identity != null)
                {
                    if(user.Identity.IsAuthenticated)
                    {
                        token = user.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
                    }
                }
            }
            return token;
        }
    }
}
