#region Comment 
/*
* [RABBIT MQ - WORKER SERVICE]
* Commented by: Jayharron Mar Abejar 11/15/2023
* 
* Implemented rabbitmq message bus for 2 services to communicate,
* make sure that RabbitMQ server is already running in Docker
* 
* This worker service will initially retrieve all questions from the repositories
* then publish it in the message bus queue. The consumer will then consume the data
* once all is settled.
* 
* This worker service will also send updates to the consumer, enabling realtime updates.
* 
* To run RabbitMQ in docker, type this in the console
* # latest RabbitMQ 3.12 (11/15/2023)
* docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
*/
#endregion
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.QuizSession.Configuration;
using QuizMaster.Library.Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace QuizMaster.API.QuizSession.Services.Workers
{
    public class QuestionSynchronizationWorkerService : BackgroundService
    {
        private readonly ILogger<QuestionSynchronizationWorkerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly QuizSessionApplicationSettings _settings;
        private readonly ConnectionFactory _RabbitMqConnectionFactory;

        public QuestionSynchronizationWorkerService(ILogger<QuestionSynchronizationWorkerService> logger, IServiceProvider serviceProvider, IOptions<QuizSessionApplicationSettings> settings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _settings = settings.Value;
            _RabbitMqConnectionFactory = new ConnectionFactory() { HostName = _settings.RabbitMq_Hostname };
            _logger.LogInformation("Enabling RabbitMQ Worker Service");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LogInformation("Initializing...");

            using IConnection connection = _RabbitMqConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            // declare an exchange
            channel.ExchangeDeclare(_settings.RabbitMq_Quiz_ExchangeName, ExchangeType.Direct);

            // Declare a request queue for recieving messages
            channel.QueueDeclare(_settings.RabbitMq_Quiz_QuizInitQueue, false, false, false, null);
            channel.QueueBind(_settings.RabbitMq_Quiz_QuizInitQueue, _settings.RabbitMq_Quiz_ExchangeName, "");

            // Setup consumer to listen for upcoming messages
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                LogInformation("Received Data");
                var body = ea.Body.ToArray(); // convert the body to byte array for deserialization
                var jsonMssage = Encoding.UTF8.GetString(body);
                var requestMessage = JsonConvert.DeserializeObject<RabbitMQ_QuestionPayload>(jsonMssage);

                if(requestMessage != null)
                {
                    LogInformation(jsonMssage);
                }
            };

            // Consume messages from the request queue
            channel.BasicConsume(_settings.RabbitMq_Quiz_QuizInitQueue, true, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void LogInformation(string message)
        {
            _logger.LogInformation($"RabbitMQ(Quiz): {message}");
        }

        private void LogCritical(string message)
        {
            _logger.LogCritical($"RabbitMQ(Quiz): {message}");
        }
    }
}
