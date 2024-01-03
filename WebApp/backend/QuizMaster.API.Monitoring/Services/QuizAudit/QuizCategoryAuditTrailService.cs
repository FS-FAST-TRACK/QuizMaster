using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public class QuizCategoryAuditTrailService : IQuizCategoryAuditTrailService
    {
        private readonly IQuizCategoryAuditTrailRepository _quizCategoryAuditTrailRepository;
        public QuizCategoryAuditTrailService(IQuizCategoryAuditTrailRepository quizCategoryAuditTrailRepository)
        {
            _quizCategoryAuditTrailRepository = quizCategoryAuditTrailRepository;
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetAllQuizCategoryAuditTrailsAsync()
        {
            try
            {
                return await _quizCategoryAuditTrailRepository.GetAllQuizCategoryAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trails.", ex);
            }
        }

        public async Task<QuizAuditTrail> GetQuizCategoryAuditTrailByIdAsync(int quizAuditTrailId)
        {
            try
            {
                return await _quizCategoryAuditTrailRepository.GetQuizCategoryAuditTrailByIdAsync(quizAuditTrailId);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _quizCategoryAuditTrailRepository.GetQuizCategoryAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _quizCategoryAuditTrailRepository.GetQuizCategoryAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _quizCategoryAuditTrailRepository.GetQuizCategoryAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by userrole.", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _quizCategoryAuditTrailRepository.GetQuizCategoryAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
