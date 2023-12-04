using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public interface IQuizDifficultyAuditTrailRepository
    {
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetAllQuizDifficultyAuditTrailsAsync();
        Task<QuizDifficultyAuditTrail> GetQuizDifficultyAuditTrailByIdAsync(int quizAuditTrailId);
        Task AddQuizDifficultyAuditTrailAsync(QuizDifficultyAuditTrail quizAuditTrail);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
