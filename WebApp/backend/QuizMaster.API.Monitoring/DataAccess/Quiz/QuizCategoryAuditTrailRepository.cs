using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public class QuizCategoryAuditTrailRepository : IQuizCategoryAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public QuizCategoryAuditTrailRepository(MonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddQuizCategoryAuditTrailAsync(QuizAuditTrail quizAuditTrail)
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

        public async Task<IEnumerable<QuizAuditTrail>> GetAllQuizCategoryAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.QuizAuditTrails.ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Failed to retrieve quiz category audit trails.", ex);
            }
        }

        public async Task<QuizAuditTrail> GetQuizCategoryAuditTrailByIdAsync(int quizAuditTrailId)
        {
            try
            {
                return await _dbContext.QuizAuditTrails.FirstOrDefaultAsync(a => a.QuizAuditTrailId == quizAuditTrailId);
            }
            catch (Exception ex) 
            {
                throw new Exception("Failed to retrieve quiz category audit trail by ID", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByActionAsync(string action)
        {
            try 
            {
                //Use Equals for exact match
                return await _dbContext.QuizAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Failed to retrive quiz category audit trails by action", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.QuizAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quiz category audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.QuizAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quiz category audit trails by user role.", ex);
            }
        }


        public async Task<IEnumerable<QuizAuditTrail>> GetQuizCategoryAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.QuizAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve quiz category audit trails within a date range.", ex);
            }
        }
    }
}
