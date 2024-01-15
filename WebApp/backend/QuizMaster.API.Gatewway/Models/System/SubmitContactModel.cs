using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.System
{
    public class SubmitContactModel
    {
        public int? UserId { get; set; } = null;
        public string Emai { get; set; } = string.Empty;
        public string PhoneNumber { get; set;} = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Firstname { get; set;} = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class ContactReaching : SubmitContactModel
    {
        [Key] 
        public int Id { get; set; }
    }
}
