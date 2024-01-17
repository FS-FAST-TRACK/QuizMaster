using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Gateway.Models.Report
{
    public class QuizReport
    {
        [Key]
        public int Id { get; set; }
        public int NoOfParticipants { get; set; }
        public int HostId { get; set; }
        public string HostName { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [JsonIgnore]
        public string ParticipantAnswerReportsJSON { get; set; } = string.Empty; // JSON form of IEnumerable<ParticipantAnswerReport>
        [JsonIgnore]
        public string LeaderboardReportsJSON { get; set; } = string.Empty; // JSON form of IEnumerable<LeaderboardReport>

        public IEnumerable<ParticipantAnswerReport>? ParticipantAnswerReports;
        public IEnumerable<LeaderboardReport>? LeaderboardReports;
    }
}
