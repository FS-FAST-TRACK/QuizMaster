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
using NuGet.Packaging;
using QuizMaster.API.Quiz.Configuration;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;
using RabbitMQ.Client;
using System.Text;

namespace QuizMaster.API.Quiz.Services.Workers
{
    public class QuizDataSynchronizationWorker : BackgroundService
    {
        private readonly ILogger<QuizDataSynchronizationWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly QuizApplicationSettings _applicationSettings;
        private readonly ConnectionFactory _connectionFactory;

        public QuizDataSynchronizationWorker(ILogger<QuizDataSynchronizationWorker> logger, IServiceProvider serviceProvider, IOptions<QuizApplicationSettings> applicationSettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _applicationSettings = applicationSettings.Value;
            _connectionFactory = new ConnectionFactory() { HostName = _applicationSettings.RabbitMq_Hostname };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool firstRun = false;
            while(!stoppingToken.IsCancellationRequested)
            {   
                try
                {
                    await Task.Delay(1000, stoppingToken);
                    LogInformation("Initializing...");

                    using IConnection connection = _connectionFactory.CreateConnection();
                    using (var channel = connection.CreateModel())
                    {
                        LogInformation("Connection Established");
                        // declare an exchange 
                        channel.ExchangeDeclare(_applicationSettings.RabbitMq_Quiz_ExchangeName, ExchangeType.Direct);

                        // Declare a queue for sending messages
                        //channel.QueueDeclare(_applicationSettings.RabbitMq_Quiz_ResponseQueueName + "Init", false, false, false, null);

                        // process the payload to be sent
                        LogInformation("Processing Data");
                        // no joke, this is heavy HAHAHAHA | Jay: I have updated to only process latest, commented: 3/6/2024
                        var processedPayload = firstRun ? await ProcessGetPayloadAsync() : await ProcessGetPayloadLatestAsync(2);

                        LogInformation("Serializing Data");
                        // serialize the payload to JSON
                        var payloadJson = JsonConvert.SerializeObject(processedPayload);
                        var payloadBody = Encoding.UTF8.GetBytes(payloadJson);

                        LogInformation("Sending Data to be consumed in other services");
                        // Publish the payload to be consumed in other worker service
                        channel.BasicPublish(_applicationSettings.RabbitMq_Quiz_ExchangeName, "", null, payloadBody);
                        LogInformation("Data was sent");

                    }
                }
                catch { }
                await Task.Delay(2_000, stoppingToken);
            }
        }


        public async Task Synchronize()
        {
            LogInformation("Initializing...");

            using IConnection connection = _connectionFactory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                LogInformation("Connection Established");
                // declare an exchange 
                channel.ExchangeDeclare(_applicationSettings.RabbitMq_Quiz_ExchangeName, ExchangeType.Direct);

                // Declare a queue for sending messages
                //channel.QueueDeclare(_applicationSettings.RabbitMq_Quiz_ResponseQueueName + "Init", false, false, false, null);

                // process the payload to be sent
                LogInformation("Processing Data");
                // no joke, this is heavy HAHAHAHA | Jay: I have updated to only process latest, commented: 3/6/2024
                var processedPayload = await ProcessGetPayloadLatestAsync(2);

                LogInformation("Serializing Data");
                // serialize the payload to JSON
                var payloadJson = JsonConvert.SerializeObject(processedPayload);
                var payloadBody = Encoding.UTF8.GetBytes(payloadJson);

                LogInformation("Sending Data to be consumed in other services");
                // Publish the payload to be consumed in other worker service
                for(int i = 0; i < 5; i++)
                    channel.BasicPublish(_applicationSettings.RabbitMq_Quiz_ExchangeName, "", null, payloadBody);
                LogInformation("Data was sent");
            }
        }

        // This should be called upon instantiation only, don't call this overtime, it might kill server
        public async Task<RabbitMQ_QuestionPayload> ProcessGetPayloadAsync()
        {
            RabbitMQ_QuestionPayload payload = new();
            using var scope = _serviceProvider.CreateScope();

            IQuizRepository? quizRepository = scope.ServiceProvider.GetService<IQuizRepository>();
            if (quizRepository != null)
            {
                payload.DetailTypes = await quizRepository.GetDetailTypesAsync();
                payload.QuestionCategories = await quizRepository.GetAllCategoriesAsync();
                payload.Questions = await quizRepository.GetAllQuestionsAsync();
                payload.QuestionDetails = await quizRepository.GetAllQuestionDetailsAsync();
                payload.QuestionDetailTypes = await quizRepository.GetAllQuestionDetailTypesAsync();
                payload.QuestionDifficulties = await quizRepository.GetAllDifficultiesAsync();
                payload.QuestionTypes = await quizRepository.GetAllTypesAsync();
            }
            return payload;
        }

        // This method will only retrieve items that are updated recently [2mins before]
        public async Task<RabbitMQ_QuestionPayload> ProcessGetPayloadLatestAsync(int minutes_recent = 2)
        {
            RabbitMQ_QuestionPayload payload = new();
            using var scope = _serviceProvider.CreateScope();

            IQuizRepository? quizRepository = scope.ServiceProvider.GetService<IQuizRepository>();
            if (quizRepository != null)
            {
                // Process DetailTypes
                IEnumerable<DetailType> detailTypes = await quizRepository.GetDetailTypesAsync();
                payload.DetailTypes = detailTypes.Where(d => d.DateUpdated > (DateTime.UtcNow - TimeSpan.FromMinutes(minutes_recent))).ToList();

                // Process QuestionCategories
                IEnumerable<QuestionCategory> questionCategories = await quizRepository.GetAllCategoriesAsync();
                payload.QuestionCategories = questionCategories.Where(c => c.DateUpdated > (DateTime.Now - TimeSpan.FromMinutes(minutes_recent))).ToList();

                // Process Questions
                IEnumerable<Question> questions = await quizRepository.GetAllQuestionsAsync();
                payload.Questions = questions.Where(q => q.DateUpdated > (DateTime.Now - TimeSpan.FromMinutes(minutes_recent))).ToList();

                // Process QuestionDetails
                IEnumerable<QuestionDetail> questionDetails = await quizRepository.GetAllQuestionDetailsAsync();
                payload.QuestionDetails = questionDetails.Where(q => q.DateUpdated > (DateTime.Now - TimeSpan.FromMinutes(minutes_recent))).ToList();

                // Process QuestionDetailTypes
                IEnumerable<QuestionDetailType> questionDetailTypes = await quizRepository.GetAllQuestionDetailTypesAsync();
                payload.QuestionDetailTypes = questionDetailTypes.Where(q => q.DateUpdated > (DateTime.Now - TimeSpan.FromMinutes(minutes_recent))).ToList();

                // Process QuestionDifficulties
                IEnumerable<QuestionDifficulty> questionDifficulties = await quizRepository.GetAllDifficultiesAsync();
                payload.QuestionDifficulties = questionDifficulties.Where(q => q.DateUpdated > (DateTime.Now - TimeSpan.FromMinutes(minutes_recent))).ToList();

                // Process QuestionTypes
                IEnumerable<QuestionType> questionTypes = await quizRepository.GetAllTypesAsync();
                payload.QuestionTypes = questionTypes.Where(q => q.DateUpdated > (DateTime.Now - TimeSpan.FromMinutes(minutes_recent))).ToList();
            }
            return payload;
        }

        private async Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            
            using var scope = _serviceProvider.CreateScope();

            IQuizRepository? quizRepository = scope.ServiceProvider.GetService<IQuizRepository>();

            if (quizRepository == null)
                return new List<Question>();

            // get all quiz
            return await quizRepository.GetAllQuestionsAsync();
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
