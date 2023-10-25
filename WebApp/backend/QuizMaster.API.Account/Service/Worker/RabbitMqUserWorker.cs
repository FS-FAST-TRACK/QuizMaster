﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Account.Configuration;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace QuizMaster.API.Account.Service.Worker
{
    /*
     * [RABBIT MQ - RECEIVER/LISTENER]
     * Commented by: Jayharron Mar Abejar 10/24/2023
     * Implemented rabbitmq message bus for 2 services to communicate,
     * make sure that RabbitMQ server is already running in Docker
     * 
     * To run RabbitMQ in docker, type this in the console
     * # latest RabbitMQ 3.12
     * docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
     */
    public class RabbitMqUserWorker : BackgroundService
    {
        private readonly ILogger<RabbitMqUserWorker> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly ApplicationSettings appSettings;
        private ConnectionFactory connectionFactory;

        // Dependency Inject to the constructor
        public RabbitMqUserWorker(ILogger<RabbitMqUserWorker> logger, IServiceProvider serviceProvider, IOptions<ApplicationSettings> appsettings)
        {
            this.serviceProvider = serviceProvider;
            this.appSettings = appsettings.Value;
            this.logger = logger;

            // instantiate rabbit mq connection factory
            connectionFactory = new ConnectionFactory() { HostName = appSettings.RabbitMq_Hostname };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("RabbitMQ: Account Worker service is now running...");

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // declare an exchange
                channel.ExchangeDeclare(appSettings.RabbitMq_Account_ExchangeName, ExchangeType.Direct);

                // Declare a request queue for recieving messages
                channel.QueueDeclare(appSettings.RabbitMq_Account_RequestQueueName, false, false, false, null);
                channel.QueueBind(appSettings.RabbitMq_Account_RequestQueueName, appSettings.RabbitMq_Account_ExchangeName, "");

                // Declare a response queue for sending responses
                channel.QueueDeclare(appSettings.RabbitMq_Account_ResponseQueueName, false, false, false, null);

                // Setup consumer to listen for upcoming messages
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray(); // convert the body to byte array for deserialization
                    var jsonMssage = Encoding.UTF8.GetString(body);
                    var requestMessage = JsonConvert.DeserializeObject<AuthRequest>(jsonMssage); // deserialize the json to object [useraccount]

                    var ProcessedPayload = ProcessRequest(requestMessage);

                    // serialize the payload to JSON
                    var jsonResponse = JsonConvert.SerializeObject(ProcessedPayload);
                    var responseBody = Encoding.UTF8.GetBytes(jsonResponse);
                    logger.LogInformation("RabbitMQ: Sending Response...\n"+ jsonResponse+"\n\n");

                    // publish the response to the response queue
                    channel.BasicPublish("", appSettings.RabbitMq_Account_ResponseQueueName, null, responseBody);
                };

                // consume messages from the request queue
                channel.BasicConsume(appSettings.RabbitMq_Account_RequestQueueName, true, consumer);

                while(!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }


        private UserAccount ProcessRequest(AuthRequest? request)
        {
            UserAccount userAccount = new() { Id = -1 };
            // Worker services do not support scoped lifetime services, that's why we will call the service provider
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<UserAccount>>();

                // return an empty user with Id of -1 when userManager was failed to invoke
                if(userManager == null)
                    return new() { Id = -1 };

                userAccount = GetUser(request, userManager);
            }

            return userAccount;
        }

        private UserAccount GetUser(AuthRequest? request, UserManager<UserAccount> userManager)
        {
            if(request == null)
                return new() { Id = -1 };

            var userAccount = userManager.Users.FirstOrDefault(u => u.Email == request.Email);

            userAccount ??= userManager.Users.FirstOrDefault(u => u.UserName == request.Username);

            if(userAccount == null)
                return new() { Id = -1 };

            // Compare user password
            PasswordHasher<UserAccount> hasher = new();

            // check if password is correct
            var passwordVerification = hasher.VerifyHashedPassword(userAccount, userAccount.PasswordHash, request.Password);
            if (PasswordVerificationResult.Success != passwordVerification) { return new() { Id = -1 }; };

            return userAccount;
        }

    }
}