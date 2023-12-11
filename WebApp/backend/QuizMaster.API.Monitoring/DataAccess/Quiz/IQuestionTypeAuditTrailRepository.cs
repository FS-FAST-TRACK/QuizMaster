using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public interface IQuestionTypeAuditTrailRepository
    {

        Task<IEnumerable<QuestionTypeAuditTrail>> GetAllQuestionTypeAuditTrailsAsync();
        Task<QuestionTypeAuditTrail> GetQuestionTypeAuditTrailByIdAsync(int id);
        Task AddQuestionTypeAuditTrailAsync(QuestionTypeAuditTrail questionTypeAuditTrail);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
