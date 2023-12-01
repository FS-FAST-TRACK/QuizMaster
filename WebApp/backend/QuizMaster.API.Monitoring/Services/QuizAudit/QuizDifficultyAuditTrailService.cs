using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public class QuizDifficultyAuditTrailService : IQuizDifficultyAuditTrailService
    {
        private readonly IQuizDifficultyAuditTrailRepository _quizDifficultyAuditTrailRepository;
        public QuizDifficultyAuditTrailService(IQuizDifficultyAuditTrailRepository quizDifficultyAuditTrailRepository)
        {
            _quizDifficultyAuditTrailRepository = quizDifficultyAuditTrailRepository;
        }
        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetAllQuizDifficultyAuditTrailsAsync()
        {
            try
            {
                return await _quizDifficultyAuditTrailRepository.GetAllQuizDifficultyAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trails.", ex);
            }
        }

        public async Task<QuizDifficultyAuditTrail> GetQuizDifficultyAuditTrailByIdAsync(int quizAuditTrailId)
        {
            try
            {
                return await _quizDifficultyAuditTrailRepository.GetQuizDifficultyAuditTrailByIdAsync(quizAuditTrailId);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _quizDifficultyAuditTrailRepository.GetQuizDifficultyAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _quizDifficultyAuditTrailRepository.GetQuizDifficultyAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _quizDifficultyAuditTrailRepository.GetQuizDifficultyAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by userrole.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _quizDifficultyAuditTrailRepository.GetQuizDifficultyAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
