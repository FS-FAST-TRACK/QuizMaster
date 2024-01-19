using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.System
{
    public class ContactModel
    {
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
    }

    public class SystemContact : ContactModel
    {
        [Key]
        public int Id { get; set; } = 0;

        public static SystemContact DEFAULT = new() { Id = 1, Email = "admin.quizmaster@gmail.com", Contact = "09205195701" };
    }
}
