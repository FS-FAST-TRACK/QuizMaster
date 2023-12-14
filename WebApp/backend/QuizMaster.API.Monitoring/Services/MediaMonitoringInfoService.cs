
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using QuizMaster.API.Monitoring.DataAccess.Media;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services
{
    public class MediaMonitoringInfoService : MediaAuditService.MediaAuditServiceBase
    {
        private readonly IMediaAuditTrailRepository _mediaAuditTrailRepository;
        private readonly ILogger<MediaMonitoringInfoService> _logger;
        public MediaMonitoringInfoService(IMediaAuditTrailRepository mediaAuditTrailRepository, ILogger<MediaMonitoringInfoService> logger)
        {
            _mediaAuditTrailRepository = mediaAuditTrailRepository;
            _logger = logger;
        }
        public override async Task<Empty> LogUploadMediaEvent(LogUploadMediaEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var mediaAuditTrail = new MediaAuditTrail
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
                    await _mediaAuditTrailRepository.AddMediaAuditTrailAsync(mediaAuditTrail);
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
        public override async Task<Empty> LogDeleteMediaEvent(LogDeleteMediaEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var mediaAuditTrail = new MediaAuditTrail
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
                    await _mediaAuditTrailRepository.AddMediaAuditTrailAsync(mediaAuditTrail);
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
