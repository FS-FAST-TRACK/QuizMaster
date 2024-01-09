using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.Room
{
    public interface IRoomAuditTrailService
    {
        Task<IEnumerable<RoomAuditTrail>> GetAllRoomAuditTrailsAsync();
        Task<RoomAuditTrail> GetRoomAuditTrailByIdAsync(int id);
        Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByActionAsync(string action);
        Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
