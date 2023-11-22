using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Configuration;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models;
using QuizMaster.Library.Common.Models.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace QuizMaster.API.Authentication.Services.Worker
{
    /*
     * [RABBIT MQ - SENDER]
     * Commented by: Jayharron Mar Abejar 10/24/2023
     * Implemented rabbitmq message bus for 2 services to communicate,
     * make sure that RabbitMQ server is already running in Docker
     * 
     * To run RabbitMQ in docker, type this in the console
     * # latest RabbitMQ 3.12
     * docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
     * 
     * Update 1.1
     * I have created a RabbitMQ_AccountPayload class which will include user roles
     */
    public class RabbitMqUserWorker
    {
        private ILogger<RabbitMqUserWorker> _logger;
        private  AppSettings AppSettings { get; set; }
        private readonly RabbitMqRepository rabbitMqRepository;

        public RabbitMqUserWorker(ILogger<RabbitMqUserWorker> logger, IOptions<AppSettings> options, RabbitMqRepository repository) 
        { 
            _logger = logger; 
            AppSettings = options.Value;
            rabbitMqRepository = repository;
        }

        public async Task<RabbitMQ_AccountPayload> RequestUserCredentials(AuthRequest authRequest)
        {

            // create the RabbitMQ connection factory
            ConnectionFactory connectionFactory = new ConnectionFactory() { HostName = AppSettings.RabbitMq_Hostname };

            // setup connection
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare an Exchange message
            channel.ExchangeDeclare(AppSettings.RabbitMq_Account_ExchangeName, ExchangeType.Direct);

            // parse the message body to request
            var jsonMessage = JsonConvert.SerializeObject(authRequest);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            // publish the message to the exchange
            channel.BasicPublish(AppSettings.RabbitMq_Account_ExchangeName, "", null, body);
            _logger.LogInformation("RabbitMQ: Sending Request to Listener services");

            // declare a response queue for receiving responses
            channel.QueueDeclare(AppSettings.RabbitMq_Account_ResponseQueueName, false, false, false, null);
            EventingBasicConsumer responseConsumer = new(channel);
            channel.BasicConsume(AppSettings.RabbitMq_Account_ResponseQueueName, true, responseConsumer);
            // deserialize the response
            responseConsumer.Received += (model, ea) =>
            {
                byte[] responseBody = ea.Body.ToArray();
                var jsonResponse = Encoding.UTF8.GetString(responseBody);
                var responseMessage = JsonConvert.DeserializeObject<RabbitMQ_AccountPayload>(jsonResponse);

                if(responseMessage != null)
                {
                    _logger.LogInformation("RabbitMQ: Recieved data");
                    rabbitMqRepository.AddCache(responseMessage);
                }
            };

            await Task.Delay(200);
            return rabbitMqRepository.TryGetCache(authRequest);
        }
    }
}
