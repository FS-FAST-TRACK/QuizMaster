using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.Library.Common.Entities.Audits;
using QuizMaster.Library.Common.Entities.Roles;
using QuizMaster.Library.Common.Helpers;

namespace QuizMaster.API.Monitoring.Services
{
    public class QuizMonitoringService : QuizAuditService.QuizAuditServiceBase
    {
        private readonly IQuizCategoryAuditTrailRepository _categoryAuditTrailRepository;
        private readonly IQuizDifficultyAuditTrailRepository _quizDifficultyAuditTrailRepository;
        private readonly IQuestionAuditTrailRepository _questionAuditTrailRepository;
        private readonly IQuestionTypeAuditTrailRepository _questionTypeAuditTrailRepository;
        private readonly ILogger<QuizMonitoringService> _logger;

        public QuizMonitoringService(IQuizCategoryAuditTrailRepository quizCategoryAuditTrailRepository, ILogger<QuizMonitoringService> logger, IQuizDifficultyAuditTrailRepository quizDifficultyAuditTrailRepository, IQuestionAuditTrailRepository questionAuditTrailRepository, IQuestionTypeAuditTrailRepository questionTypeAuditTrailRepository)
        {
            _categoryAuditTrailRepository = quizCategoryAuditTrailRepository;
            _logger = logger;
            _quizDifficultyAuditTrailRepository = quizDifficultyAuditTrailRepository;
            _questionAuditTrailRepository = questionAuditTrailRepository;
            _questionTypeAuditTrailRepository = questionTypeAuditTrailRepository;
        }
        public override async Task<Empty> LogCreateQuizCategoryEvent(LogCreateQuizCategoryEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var quizCategoryAuditTrail = new QuizAuditTrail
                    {
                       
                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _categoryAuditTrailRepository.AddQuizCategoryAuditTrailAsync(quizCategoryAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogUpdateQuizCategoryEvent(LogUpdateQuizCategoryEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var quizCategoryAuditTrail = new QuizAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _categoryAuditTrailRepository.AddQuizCategoryAuditTrailAsync(quizCategoryAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogDeleteQuizCategoryEvent(LogDeleteQuizCategoryEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var quizCategoryAuditTrail = new QuizAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _categoryAuditTrailRepository.AddQuizCategoryAuditTrailAsync(quizCategoryAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogCreateQuizDifficultyEvent(LogCreateQuizDifficultyEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var quizDifficultyAuditTrail = new QuizDifficultyAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _quizDifficultyAuditTrailRepository.AddQuizDifficultyAuditTrailAsync(quizDifficultyAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogUpdateQuizDifficultyEvent(LogUpdateQuizDifficultyEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var quizDifficultyAuditTrail = new QuizDifficultyAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _quizDifficultyAuditTrailRepository.AddQuizDifficultyAuditTrailAsync(quizDifficultyAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogDeleteQuizDifficultyEvent(LogDeleteQuizDifficultyEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var quizDifficultyAuditTrail = new QuizDifficultyAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _quizDifficultyAuditTrailRepository.AddQuizDifficultyAuditTrailAsync(quizDifficultyAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }
        }
        public override async Task<Empty> LogCreateQuestionEvent(LogCreateQuestionEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var questionAuditTrail = new QuestionAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _questionAuditTrailRepository.AddQuestionAuditTrailAsync(questionAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogUpdateQuestionEvent(LogUpdateQuestionEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var questionAuditTrail = new QuestionAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _questionAuditTrailRepository.AddQuestionAuditTrailAsync(questionAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogDeleteQuestionEvent(LogDeleteQuestionEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var questionAuditTrail = new QuestionAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _questionAuditTrailRepository.AddQuestionAuditTrailAsync(questionAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogCreateQuestionTypeEvent(LogCreateQuestionTypeEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var questionTypeAuditTrail = new QuestionTypeAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _questionTypeAuditTrailRepository.AddQuestionTypeAuditTrailAsync(questionTypeAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogUpdateQuestionTypeEvent(LogUpdateQuestionTypeEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var questionTypeAuditTrail = new QuestionTypeAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _questionTypeAuditTrailRepository.AddQuestionTypeAuditTrailAsync(questionTypeAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogDeleteQuestionTypeEvent(LogDeleteQuestionTypeEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var questionTypeAuditTrail = new QuestionTypeAuditTrail
                    {

                        UserId = request.Event.UserId,
                        UserName = request.Event.Username,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _questionTypeAuditTrailRepository.AddQuestionTypeAuditTrailAsync(questionTypeAuditTrail);
                }
                else
                {
                    // Handle the case where the timestamp is not a valid date and time
                    _logger.LogError("Invalid timestamp format: {request.Timestamp}");
                    // You can log an error or use a default timestamp as needed.
                }

                return new Empty();
            }
            catch (Exception ex)
            {
                // Handle any exceptions, log them, and return an appropriate response
                Console.WriteLine($"Error while logging add event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
    }      
    
}
