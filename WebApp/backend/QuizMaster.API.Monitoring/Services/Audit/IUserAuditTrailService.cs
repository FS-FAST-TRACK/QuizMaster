
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.Audit
{
    public interface IUserAuditTrailService
    {
        Task<IEnumerable<UserAuditTrail>> GetAllUserAuditTrailsAsync();
        Task<UserAuditTrail> GetUserAuditTrailByIdAsync(int auditTrailId);
        Task AddAuditTrailAsync(UserAuditTrail auditTrail);
        Task DeleteAuditTrailAsync(int auditTrailId);
        Task<IEnumerable<UserAuditTrail>> GetAuditTrailsForUserAsync(int userId);
        Task<IEnumerable<UserAuditTrail>> GetAuditTrailsByActionAsync(string action);
        Task<IEnumerable<UserAuditTrail>> GetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}