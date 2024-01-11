namespace QuizMaster.API.Account.Configuration
{
    public class ApplicationSettings
    {
        public string RabbitMq_Account_ExchangeName { get; set; }
        public string RabbitMq_Account_RequestQueueName { get; set; }
        public string RabbitMq_Account_ResponseQueueName { get; set; }
        public string RabbitMq_Hostname { get; set; }
        public string SMTP_EMAIL { get; set; }
        public string SMTP_PASSWORD { get; set; }
    }
}
