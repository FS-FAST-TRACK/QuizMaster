using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public class QuizSetAuditTrailRepository : IQuizSetAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public QuizSetAuditTrailRepository(MonitoringDbContext dbContext)
        {

            _dbContext = dbContext;

        }

        public async Task AddQuizSetAuditTrailAsync(QuizSetAuditTrail quizSetAuditTrail)
        {
            try
            {
                _dbContext.Add(quizSetAuditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add quizset audit trail.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetAllQuizSetAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.QuizSetAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quizset audit trails.", ex);
            }
        }

        public async Task<QuizSetAuditTrail> GetQuizSetAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _dbContext.QuizSetAuditTrails.FirstOrDefaultAsync(a => a.QuizSetAuditTrailId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quizset audit trails by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.QuizSetAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quizset audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.QuizSetAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quizset audit trails by user role.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.QuizSetAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve quizset audit trails within a date range.", ex);
            }
        }

        public async Task<IEnumerable<QuizSetAuditTrail>> GetQuizSetAuditTrailsByActionAsync(string action)
        {
            try
            {
                //Use Equals for exact match
                return await _dbContext.QuizSetAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrive quiz set audit trails by action", ex);
            }
        }
    }
}
