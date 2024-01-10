using QuizMaster.API.Monitoring.DataAccess.Room;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.Room
{
    public class RoomAuditTrailService : IRoomAuditTrailService
    {
        private readonly IRoomAuditTrailRepository _roomAuditTrailRepository;

        public RoomAuditTrailService(IRoomAuditTrailRepository roomAuditTrailRepository)
        {
            _roomAuditTrailRepository = roomAuditTrailRepository;
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetAllRoomAuditTrailsAsync()
        {
            try
            {
                return await _roomAuditTrailRepository.GetAllRoomAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve room audit trails.", ex);
            }
        }

        public async Task<RoomAuditTrail> GetRoomAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _roomAuditTrailRepository.GetRoomAuditTrailByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _roomAuditTrailRepository.GetRoomAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _roomAuditTrailRepository.GetRoomAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _roomAuditTrailRepository.GetRoomAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by user Role.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _roomAuditTrailRepository.GetRoomAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
