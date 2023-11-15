namespace QuizMaster.API.QuizSession.Models
{
    public class Notification
    {
        public string Type { get; set; } = string.Empty; // E.g: PlayerJoin, PlayerLeave, PlayerInfo
        public string Message { get; set; } = string.Empty; // value associated with the type
    }
}
