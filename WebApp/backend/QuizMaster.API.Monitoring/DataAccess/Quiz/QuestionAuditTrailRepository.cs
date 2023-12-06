using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public class QuestionAuditTrailRepository : IQuestionAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public QuestionAuditTrailRepository(MonitoringDbContext dbContext)
        {

            _dbContext = dbContext;

        }
        public async Task AddQuestionAuditTrailAsync(QuestionAuditTrail questionAuditTrail)
        {
            try 
            {
                _dbContext.Add(questionAuditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add question audit trail.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetAllQuestionAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.QuestionAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question audit trails.", ex);
            }
        }

        public async Task<QuestionAuditTrail> GetQuestionAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _dbContext.QuestionAuditTrails.FirstOrDefaultAsync(a => a.QuestionAuditTrailId == id);
            }
            catch (Exception ex) 
            {
                throw new Exception("Failed to retrieve question audit trails by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByActionAsync(string action)
        {
            try
            {
                //Use Equals for exact match
                return await _dbContext.QuestionAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrive quiz question audit trails by action", ex);
            }

        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.QuestionAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.QuestionAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question audit trails by user role.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.QuestionAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve question audit trails within a date range.", ex);
            }
        }
    }
}
