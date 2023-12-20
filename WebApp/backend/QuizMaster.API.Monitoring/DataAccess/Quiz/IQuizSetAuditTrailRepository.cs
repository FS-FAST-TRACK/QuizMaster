using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public interface IQuizSetAuditTrailRepository
    {
        Task<IEnumerable<QuizSetAuditTrail>> GetAllQuizSetAuditTrailsAsync();
        Task<QuizSetAuditTrail> GetQuizSetAuditTrailByIdAsync(int id);
        Task AddQuizSetAuditTrailAsync(QuizSetAuditTrail quizSetAuditTrail);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
