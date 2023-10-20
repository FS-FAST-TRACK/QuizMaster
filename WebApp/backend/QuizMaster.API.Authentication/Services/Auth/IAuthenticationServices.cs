using QuizMaster.API.Authentication.Models;

namespace QuizMaster.API.Authentication.Services.Auth
{
    public interface IAuthenticationServices
    {
        AuthResponse Authenticate(AuthRequest authRequest);

        AuthStore? Validate(string token);
    }
}
