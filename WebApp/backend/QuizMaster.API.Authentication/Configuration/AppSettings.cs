namespace QuizMaster.API.Authentication.Configuration
{
    public class AppSettings
    {
        public string JWTSecret { get; set; } = string.Empty;
        public int IntExpireHour { get; set; } = 1;
        public string RabbitMq_Account_ExchangeName { get; set; }
        public string RabbitMq_Account_RequestQueueName { get; set; }
        public string RabbitMq_Account_ResponseQueueName { get; set; }
        public string RabbitMq_Hostname { get; set; }
    }
}
