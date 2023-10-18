using QuizMaster.API.Authentication.Models;

namespace QuizMaster.API.Authentication.Services.Auth
{
    public interface IAuthenticationServices
    {
        AuthResponse Authenticate(AuthRequest authRequest);
    }
}
