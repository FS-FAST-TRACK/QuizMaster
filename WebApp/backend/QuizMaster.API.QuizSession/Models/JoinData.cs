using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.QuizSession.Models
{
    public class JoinData
    {
        [Required]
        public string ConnectionId { get; set; } = string.Empty;
        [Required]
        public string DisplayName { get; set; } = string.Empty;
    }
}
