using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MonitoringService.DataAccess;
using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services
{
    public class MonitoringInfoService : AuditService.AuditServiceBase
    {
        private readonly MonitoringDbContext _dbContext;
        private readonly UserAuditTrailRepository _auditTrailRepository;
        private readonly ILogger<MonitoringInfoService> _logger;

        public MonitoringInfoService(MonitoringDbContext dbContext, UserAuditTrailRepository auditTrailRepository, ILogger<MonitoringInfoService> logger)
        {
            _dbContext = dbContext;
            _auditTrailRepository = auditTrailRepository;
            _logger = logger;
        }

        public override async Task<Empty> LogRegistrationEvent(LogRegistrationEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    // Create an AuditTrail object from the RegistrationEvent
                    var userAuditTrail = new UserAuditTrail
                    {
                        UserId = request.Event.UserId,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues,
                       
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _auditTrailRepository.AddAuditTrailAsync(userAuditTrail);
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
                Console.WriteLine($"Error while logging registration event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }
        }
        public override async Task<Empty> LogPartialRegistrationEvent(LogPartialRegistrationEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    // Create an AuditTrail object from the RegistrationEvent
                    var userAuditTrail = new UserAuditTrail
                    {
                        UserId = request.Event.UserId,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues,
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _auditTrailRepository.AddAuditTrailAsync(userAuditTrail);
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
                Console.WriteLine($"Error while logging registration event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }
        }

        public override async Task<Empty> LogPasswordChangeEvent(LogPasswordChangeEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    // Create an AuditTrail object from the PasswordChangeEvent
                    var userAuditTrail = new UserAuditTrail
                    {
                        UserId = request.Event.UserId,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        //Details = request.Event.Details,
                        // UserRole = request.Event.Userrole,
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _auditTrailRepository.AddAuditTrailAsync(userAuditTrail);
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
                Console.WriteLine($"Error while logging password change event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }
        }
        public override async Task<Empty> LogSetAdminEvent(LogSetAdminEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    // Create an AuditTrail object from the SetAdminEvent
                    var userAuditTrail = new UserAuditTrail
                    {
                        UserId = request.Event.UserId,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _auditTrailRepository.AddAuditTrailAsync(userAuditTrail);
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
                Console.WriteLine($"Error while logging delete event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }

        }
        public override async Task<Empty> LogDeleteEvent(LogDeleteEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    // Create an AuditTrail object from the DeleteEvent
                    var userAuditTrail = new UserAuditTrail
                    {
                        UserId = request.Event.UserId,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _auditTrailRepository.AddAuditTrailAsync(userAuditTrail);
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
                Console.WriteLine($"Error while logging delete event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }
        }

        public override async Task<Empty> LogUpdateEvent(LogUpdateEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    // Create an AuditTrail object from the UpdateEvent
                    var userAuditTrail = new UserAuditTrail
                    {
                        UserId = request.Event.UserId,
                        Action = request.Event.Action,
                        Timestamp = timestamp,
                        Details = request.Event.Details,
                        UserRole = request.Event.Userrole,
                        OldValues = request.Event.OldValues,
                        NewValues = request.Event.NewValues
                        // You can add more fields or details as needed
                    };

                    // Add the auditTrail to the database using Entity Framework Core
                    await _auditTrailRepository.AddAuditTrailAsync(userAuditTrail);
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
                Console.WriteLine($"Error while logging update event: {ex.Message}");
                throw; // You may want to return an error response instead of throwing an exception.
            }
        }
    }
}
