using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public interface IQuizCategoryAuditTrailRepository
    {
        Task<IEnumerable<QuizAuditTrail>> GetAllQuizCategoryAuditTrailsAsync();
        Task<QuizAuditTrail> GetQuizCategoryAuditTrailByIdAsync(int quizAuditTrailId);
        Task AddQuizCategoryAuditTrailAsync(QuizAuditTrail quizAuditTrail);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
