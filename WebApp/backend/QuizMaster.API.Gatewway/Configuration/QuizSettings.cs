namespace QuizMaster.API.Gateway.Configuration
{
    public class QuizSettings
    {
        public int ShowAnswerAfterQuestionDelay { get; set; } = 10;
        public int ForceNextRoundTimeout { get; set; } = 300;
        public int BufferTime { get; set; } = 0;

        public OverrideQuestionTimer OverrideQuestionTimer { get; set;} = new();
        public OverridePointSystem OverridePointSystem { get; set; } = new();
    }

    public class OverrideQuestionTimer 
    {
        public int TypeAnswer { get; set; } = 0;
        public int MultipleChoice { get; set; } = 0;
        public int TrueOrFalse { get; set; } = 0;
    }


    public class OverridePointSystem
    {
        public int Easy { get; set; } = 1;
        public int Average { get; set; } = 3;
        public int Difficult { get; set; } = 5;
        public int Clincher { get; set; } = 5;
        public int GeneralPoints { get; set; } = 1;   
    }
}
