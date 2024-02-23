using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.Report
{
    public class ParticipantAnswerReport
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public int? ParticipantId { get; set; } = null;
        public string ParticipantName { get; set; } = string.Empty; // can be username
        public string Answer { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public string ScreenshotLink { get; set; } = string.Empty;
    }
}
