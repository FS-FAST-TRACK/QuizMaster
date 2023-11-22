using QuizMaster.API.Authentication.Models;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Authentication.Services.Auth
{
    public interface IAuthenticationServices
    {
        Task<AuthResponse> Authenticate(AuthRequest authRequest);

        AuthStore? Validate(string token);

        Task<ResponseDto> UpdateRole(AuthRequest authRequest, bool SetAdmin);
    }
}
