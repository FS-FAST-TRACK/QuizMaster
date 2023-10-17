namespace QuizMaster.API.Authentication.Configuration
{
    public class AppSettings
    {
        public string JWTSecret { get; set; } = string.Empty;
        public int IntExpireHour { get; set; } = 1;
    }
}
