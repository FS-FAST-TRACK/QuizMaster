using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using QuizMaster.API.Monitoring.DataAccess.Room;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services
{
    public class RoomMonitoringInfoService : RoomAuditService.RoomAuditServiceBase
    {
        private readonly IRoomAuditTrailRepository _roomAuditTrailRepository;
        private readonly ILogger<RoomMonitoringInfoService> _logger;

        public RoomMonitoringInfoService(IRoomAuditTrailRepository roomAuditTrailRepository, ILogger<RoomMonitoringInfoService> logger)
        {
            _roomAuditTrailRepository = roomAuditTrailRepository;
            _logger = logger;
        }

        public override async Task<Empty> LogCreateRoomEvent(LogCreateRoomEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var roomAuditTrail = new RoomAuditTrail
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
                    await _roomAuditTrailRepository.AddRoomAuditTrailAsync(roomAuditTrail);
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

        public override async Task<Empty> LogUpdateRoomEvent(LogUpdateRoomEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var roomAuditTrail = new RoomAuditTrail
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
                    await _roomAuditTrailRepository.AddRoomAuditTrailAsync(roomAuditTrail);
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

        public override async Task<Empty> LogDeleteRoomEvent(LogDeleteRoomEventRequest request, ServerCallContext context)
        {
            try
            {
                if (DateTime.TryParse(request.Event.Timestamp, out DateTime timestamp))
                {
                    var roomAuditTrail = new RoomAuditTrail
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
                    await _roomAuditTrailRepository.AddRoomAuditTrailAsync(roomAuditTrail);
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
