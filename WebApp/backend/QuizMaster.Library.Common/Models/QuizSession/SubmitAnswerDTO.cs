namespace QuizMaster.Library.Common.Models.QuizSession
{
    public class SubmitAnswerDTO
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
    }
}
