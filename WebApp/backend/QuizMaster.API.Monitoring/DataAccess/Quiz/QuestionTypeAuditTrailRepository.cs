using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Quiz
{
    public class QuestionTypeAuditTrailRepository : IQuestionTypeAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public QuestionTypeAuditTrailRepository(MonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddQuestionTypeAuditTrailAsync(QuestionTypeAuditTrail questionTypeAuditTrail)
        {
            try
            {
                _dbContext.Add(questionTypeAuditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add question audit trail.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetAllQuestionTypeAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.QuestionTypeAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question audit trails.", ex);
            }
        }

        public async Task<QuestionTypeAuditTrail> GetQuestionTypeAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _dbContext.QuestionTypeAuditTrails.FirstOrDefaultAsync(a => a.QuestionTypeAuditTrailId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question audit trails by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByActionAsync(string action)
        {
            try
            {
                //Use Equals for exact match
                return await _dbContext.QuestionTypeAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrive  question type audit trails by action", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.QuestionTypeAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question type audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.QuestionTypeAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve question type audit trails by user role.", ex);
            }
        }

        public async Task<IEnumerable<QuestionTypeAuditTrail>> GetQuestionTypeAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.QuestionTypeAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve question type audit trails within a date range.", ex);
            }
        }
    }
}
