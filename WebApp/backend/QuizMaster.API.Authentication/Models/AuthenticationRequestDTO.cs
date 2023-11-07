using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Authentication.Models
{
    /// <summary>
    /// ## Authentication Request
    /// 
    /// This **DTO** will be used for logging in purpose
    /// </summary>
    public class AuthenticationRequestDTO
    {
        /// <summary>
        /// **Username**(Optional when Email exists): Authenticatate with using the user's Username
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// **Email**(Optional when Username exists): Authenticatate with using the user's Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// **Password**(Required): Used to authenticate the user
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;

        public AuthRequest GetAuthRequest()
        {
            return new AuthRequest { Email = Email, Password = Password, Username = Username, Type = "Credentials" };
        }
    }
}
