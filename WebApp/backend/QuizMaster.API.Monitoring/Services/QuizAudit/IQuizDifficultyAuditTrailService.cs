using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public interface IQuizDifficultyAuditTrailService
    {
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetAllQuizDifficultyAuditTrailsAsync();
        Task<QuizDifficultyAuditTrail> GetQuizDifficultyAuditTrailByIdAsync(int quizAuditTrailId);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
