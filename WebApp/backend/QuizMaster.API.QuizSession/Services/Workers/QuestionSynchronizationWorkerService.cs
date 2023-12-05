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
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
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
                    ProcessData(requestMessage);
                }
            };

            // Consume messages from the request queue
            channel.BasicConsume(_settings.RabbitMq_Quiz_QuizInitQueue, true, consumer);

            LogInformation("Message Bus running");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void ProcessData(RabbitMQ_QuestionPayload questionPayload)
        {
            // create a scope
            using var scope = _serviceProvider.CreateScope();

            IQuizSessionRepository? quizSessionRepository = scope.ServiceProvider.GetService<IQuizSessionRepository>();

            if(quizSessionRepository == null)
            {
                LogCritical("Failed to Process the data Received... QuizSessionRepository service is not available");
                return;
            }

            using var context = quizSessionRepository.QuizSessionDbContext;

            #region Difficulties 
            // Save the Difficulties
            questionPayload.QuestionDifficulties.ToList().ForEach(difficulty =>
            {
                var _item = context.Difficulties.FirstOrDefault(d => d.Id == difficulty.Id);
                if (_item == null)
                    context.Difficulties.Add(difficulty);
                else
                {
                    new Copier<QuestionDifficulty>().SoftCopy(_item, difficulty);
                    context.Difficulties.Update(_item);
                }
                context.SaveChanges();
            });
            #endregion


            #region Categories 
            // Save the Categories
            questionPayload.QuestionCategories.ToList().ForEach(category =>
            {
                var _item = context.Categories.FirstOrDefault(c => c.Id == category.Id);
                LogInformation(JsonConvert.SerializeObject(_item));
                // add the item if does not exists
                if (_item == null)
                    context.Categories.Add(category);
                else // otherwise, update the existing data
                {
                    new Copier<QuestionCategory>().SoftCopy(_item, category);
                    context.Categories.Update(_item);
                }
                context.SaveChanges();
            });
            #endregion

            #region Question Types 
            // Save the Question Types
            questionPayload.QuestionTypes.ToList().ForEach(type =>
            {
                var _item = context.Types.FirstOrDefault(d => d.Id == type.Id);
                if (_item == null)
                    context.Types.Add(type);
                else
                {
                    new Copier<QuestionType>().SoftCopy(_item, type);
                    context.Types.Update(_item);
                }
                context.SaveChanges();
            });
            #endregion

            #region Detail Types
            // Save the DetailTypes
            questionPayload.DetailTypes.ToList().ForEach(detailType =>
            {
                var _item = context.DetailTypes.FirstOrDefault(d => d.Id == detailType.Id);
                if (_item == null)
                    context.DetailTypes.Add(detailType);
                else
                {
                    new Copier<DetailType>().SoftCopy(_item, detailType);
                    context.DetailTypes.Update(_item);
                }
                context.SaveChanges();
            });
            #endregion

            #region Question 
            // Save the question
            
            questionPayload.Questions.ToList().ForEach(q =>
            {
                q.QCategoryId = q.QCategory.Id;
                q.QCategory = null;
                q.QDifficultyId = q.QDifficulty.Id;
                q.QDifficulty = null;
                q.QTypeId = q.QType.Id;
                q.QType = null;
                var _item = context.Questions.FirstOrDefault(question => question.Id == q.Id);
                if (_item == null)
                    context.Questions.Add(q);
                else
                {
                    new Copier<Question>().SoftCopy(_item, q);
                    context.Questions.Update(_item);
                }

                context.SaveChanges();
            });
            #endregion

            

            #region Question Details 
            // Save the Details
            questionPayload.QuestionDetails.ToList().ForEach(detail =>
            {
				detail.QuestionId = detail.Question != null ? detail.Question.Id : detail.QuestionId;
				detail.Question = null;
                var _item = context.QuestionDetails.FirstOrDefault(d => d.Id == detail.Id);
                if (_item == null)
                    context.QuestionDetails.Add(detail);
                else
                {
                    new Copier<QuestionDetail>().SoftCopy(_item, detail);
                    context.QuestionDetails.Update(_item);
                }
                context.SaveChanges();
            });
            #endregion

            #region Question Detail Types
            // Save the Question Detail Types
            questionPayload.QuestionDetailTypes.ToList().ForEach(dType =>
            {
                dType.QuestionDetailId = dType.QuestionDetail.Id;
                dType.QuestionDetail = null;
                var _item = context.QuestionDetailTypes.FirstOrDefault(d => d.DetailTypeId == dType.DetailTypeId && d.QuestionDetailId == dType.QuestionDetailId);
                if (_item == null)
                    context.QuestionDetailTypes.Add(dType);
                else
                {
                    new Copier<QuestionDetailType>().SoftCopy(_item, dType);
                    context.QuestionDetailTypes.Update(_item);
                }
                context.SaveChanges();
            });
            #endregion
            var questions = context.Questions.ToList();
            LogInformation("Data Synchronized");
            LogInformation(JsonConvert.SerializeObject(questions));

        }

        private void LogInformation(string message)
        {
            _logger.LogInformation($"RabbitMQ(QuizSession): {message}");
        }

        private void LogCritical(string message)
        {
            _logger.LogCritical($"RabbitMQ(QuizSession): {message}");
        }

        class Copier<C>
        {
            public C SoftCopy(C source, C target)
            {
                foreach (PropertyInfo prop in typeof(C).GetProperties())
                {
                    var method_get = prop.GetMethod;
                    var method_set = prop.SetMethod;

                    if (method_get == null || method_set == null || !method_set.IsPublic)
                    {
                        continue;
                    }

                    var val = method_get.Invoke(target, null);

                    if (val == null)
                    {
                        continue;
                    }

                    method_set.Invoke(source, new object[] { val });
                }

                return source;
            }
        }
    }
}
