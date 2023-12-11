using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public interface IQuestionTypeAuditTrailService
    {
        Task<IEnumerable<QuestionTypeAuditTrail>> GetAllQuestionTypeAuditTrailsAsync();
        Task<QuestionTypeAuditTrail> GetQuestionTypeAuditTrailByIdAsync(int id);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
