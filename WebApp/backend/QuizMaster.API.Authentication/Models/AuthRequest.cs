using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Authentication.Models
{
    public class AuthRequest
    {
        public string Type { get; set; } = "Credentials";
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;


    }
}
