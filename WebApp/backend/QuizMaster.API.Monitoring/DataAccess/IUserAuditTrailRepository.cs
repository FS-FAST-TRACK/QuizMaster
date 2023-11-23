using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess
{
    public interface IUserAuditTrailRepository
    {
        Task<IEnumerable<UserAuditTrail>> GetAllUserAuditTrailsAsync();
        Task<UserAuditTrail> GetUserAuditTrailByIdAsync(int auditTrailId);
        Task AddAuditTrailAsync(UserAuditTrail userAuditTrail);
        Task DeleteAuditTrailAsync(int userAuditTrailId);
        Task<IEnumerable<UserAuditTrail>> GetAuditTrailsForUserAsync(int userId);
        Task<IEnumerable<UserAuditTrail>> GetAuditTrailsByActionAsync(string action);
        Task<IEnumerable<UserAuditTrail>> GetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
