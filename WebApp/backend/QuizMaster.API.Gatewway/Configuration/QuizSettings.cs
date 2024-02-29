namespace QuizMaster.API.Gateway.Configuration
{
    public class QuizSettings
    {
        public int ShowAnswerAfterQuestionDelay { get; set; } = 10;
        public int ForceNextRoundTimeout { get; set; } = 300;
    }
}
