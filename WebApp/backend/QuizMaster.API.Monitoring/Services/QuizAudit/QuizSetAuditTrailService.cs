using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public class QuizSetAuditTrailService : IQuizSetAuditTrailService
    {
        private readonly IQuizSetAuditTrailRepository _quizSetAuditTrailRepository;

        public QuizSetAuditTrailService(IQuizSetAuditTrailRepository quizSetAuditTrailRepository)
        {
            _quizSetAuditTrailRepository = quizSetAuditTrailRepository;
        }
        public async Task<IEnumerable<QuizSetAuditTrail>> GetAllQuizSetAuditTrailsAsync()
        {
            try
            {
                return await _quizSetAuditTrailRepository.GetAllQuizSetAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve quizset audit trails.", ex);
            }
        }

        public async Task<QuizSetAuditTrail> GetQuizSetAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _quizSetAuditTrailRepository.GetQuizSetAuditTrailByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _quizSetAuditTrailRepository.GetQuizSetAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _quizSetAuditTrailRepository.GetQuizSetAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _quizSetAuditTrailRepository.GetQuizSetAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by user Role.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _quizSetAuditTrailRepository.GetQuizSetAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
