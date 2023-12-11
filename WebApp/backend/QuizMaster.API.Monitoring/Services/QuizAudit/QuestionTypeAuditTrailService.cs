using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public class QuestionTypeAuditTrailService : IQuestionTypeAuditTrailService
    {
        private readonly IQuestionTypeAuditTrailRepository _questionTypeAuditTrailRepository;
        public QuestionTypeAuditTrailService(IQuestionTypeAuditTrailRepository questionTypeAuditTrailRepository)
        {
            _questionTypeAuditTrailRepository = questionTypeAuditTrailRepository;
        }
        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetAllQuestionTypeAuditTrailsAsync()
        {
            try
            {
                return await _questionTypeAuditTrailRepository.GetAllQuestionTypeAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve question type audit trails.", ex);
            }
        }

        public async Task<QuestionTypeAuditTrail> GetQuestionTypeAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _questionTypeAuditTrailRepository.GetQuestionTypeAuditTrailByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }

        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _questionTypeAuditTrailRepository.GetQuestionTypeAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _questionTypeAuditTrailRepository.GetQuestionTypeAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _questionTypeAuditTrailRepository.GetQuestionTypeAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by user Role.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _questionTypeAuditTrailRepository.GetQuestionTypeAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
