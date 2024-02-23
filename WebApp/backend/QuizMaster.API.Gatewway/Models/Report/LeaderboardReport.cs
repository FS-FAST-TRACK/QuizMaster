namespace QuizMaster.API.Gateway.Models.Report
{
    public class LeaderboardReport
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public int Score { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
    }
}
