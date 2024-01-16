using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.Report
{
    public class QuizReport
    {
        [Key]
        public int Id { get; set; }
        public int NoOfParticipants { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ParticipantAnswerReportsJSON { get; set; } = string.Empty; // JSON form of IEnumerable<ParticipantAnswerReport>
        public string LeaderboardReportsJSON { get; set; } = string.Empty; // JSON form of IEnumerable<LeaderboardReport>
    }
}
