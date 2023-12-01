using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public interface IQuizCategoryAuditTrailService
    {
        Task<IEnumerable<QuizAuditTrail>> GetAllQuizCategoryAuditTrailsAsync();
        Task<QuizAuditTrail> GetQuizCategoryAuditTrailByIdAsync(int quizAuditTrailId);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
