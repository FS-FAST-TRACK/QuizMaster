using QuizMaster.API.Authentication.Models;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Authentication.Services.Auth
{
    public interface IAuthenticationServices
    {
        AuthResponse Authenticate(AuthRequest authRequest);

        AuthStore? Validate(string token);

        ResponseDto UpdateRole(AuthRequest authRequest);
    }
}
