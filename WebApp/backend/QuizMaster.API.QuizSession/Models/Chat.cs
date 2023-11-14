namespace QuizMaster.API.QuizSession.Models
{
    public class Chat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
