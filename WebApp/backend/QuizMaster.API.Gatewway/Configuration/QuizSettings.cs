namespace QuizMaster.API.Gateway.Configuration
{
    public class QuizSettings
    {
        public int ShowAnswerAfterQuestionDelay { get; set; } = 10;
        public int ForceNextRoundTimeout { get; set; } = 300;

        public OverrideQuestionTimer OverrideQuestionTimer { get; set;} = new();
    }

    public class OverrideQuestionTimer 
    {
        public int TypeAnswer { get; set; } = 0;
        public int MultipleChoice { get; set; } = 0;
        public int TrueOrFalse { get; set; } = 0;
    }
}
