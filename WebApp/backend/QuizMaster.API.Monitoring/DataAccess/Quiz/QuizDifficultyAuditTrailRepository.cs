using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public class QuizDifficultyAuditTrailRepository : IQuizDifficultyAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public QuizDifficultyAuditTrailRepository(MonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddQuizDifficultyAuditTrailAsync(QuizDifficultyAuditTrail quizAuditTrail)
        {
            try
            {
                _dbContext.Add(quizAuditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to add quiz audit trail.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetAllQuizDifficultyAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.QuizDifficultyAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quiz difficulty audit trails.", ex);
            }
        }

        public async Task<QuizDifficultyAuditTrail> GetQuizDifficultyAuditTrailByIdAsync(int quizAuditTrailId)
        {
            try
            {
                return await _dbContext.QuizDifficultyAuditTrails.FirstOrDefaultAsync(a => a.QuizDiffAuditTrailId == quizAuditTrailId);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quiz difficulty audit trail by ID", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByActionAsync(string action)
        {
            try
            {
                //Use Equals for exact match
                return await _dbContext.QuizDifficultyAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrive quiz difficulty audit trails by action", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.QuizDifficultyAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quiz difficulty audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.QuizDifficultyAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quiz difficulty audit trails by user role.", ex);
            }
        }

        public async Task<IEnumerable<QuizDifficultyAuditTrail>> GetQuizDifficultyAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.QuizDifficultyAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve quiz difficulty audit trails within a date range.", ex);
            }
        }
    }
}
