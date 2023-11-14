using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.QuizSession.Models
{
    public class ChatPost
    {
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public int RoomId { get;set; }
    }
}
