using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public interface IQuizSetAuditTrailService
    {
        Task<IEnumerable<QuizSetAuditTrail>> GetAllQuizSetAuditTrailsAsync();
        Task<QuizSetAuditTrail> GetQuizSetAuditTrailByIdAsync(int id);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
