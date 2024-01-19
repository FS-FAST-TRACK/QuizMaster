using QuizMaster.API.Authentication.Configuration;

namespace QuizMaster.API.Gateway.Configuration
{
    public class ApplicationSettings: AppSettings
    {
        public string SMTP_EMAIL { get; set; }
        public string SMTP_PASSWORD { get; set; }
    }
}
