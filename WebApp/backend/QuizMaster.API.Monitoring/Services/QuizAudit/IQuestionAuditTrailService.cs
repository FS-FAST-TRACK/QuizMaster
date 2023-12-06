using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public interface IQuestionAuditTrailService
    {
        Task<IEnumerable<QuestionAuditTrail>> GetAllQuestionAuditTrailsAsync();
        Task<QuestionAuditTrail> GetQuestionAuditTrailByIdAsync(int id);
        Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByActionAsync(string action);
        Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
