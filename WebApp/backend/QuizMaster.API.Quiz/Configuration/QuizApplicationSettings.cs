﻿namespace QuizMaster.API.Quiz.Configuration
{
    public class QuizApplicationSettings
    {
        public string RabbitMq_Quiz_ExchangeName { get; set; } = string.Empty;
        public string RabbitMq_Quiz_QuizInitQueue { get; set; } = string.Empty;
        public string RabbitMq_Hostname { get; set; } = string.Empty;
    }
}
