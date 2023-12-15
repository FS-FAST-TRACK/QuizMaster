using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Media
{
    public class MediaAuditTrailRepository : IMediaAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public MediaAuditTrailRepository(MonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddMediaAuditTrailAsync(MediaAuditTrail mediaAuditTrail)
        {
            try 
            {
                _dbContext.Add(mediaAuditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add media audit trail.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetAllMediaAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.MediaAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve media audit trails.", ex);
            }
        }

        public async Task<MediaAuditTrail> GetMediaAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _dbContext.MediaAuditTrails.FirstOrDefaultAsync(a => a.MediaAuditTrailId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve media audit trails by ID.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByActionAsync(string action)
        {
            try
            {
                //Use Equals for exact match
                return await _dbContext.MediaAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrive media audit trails by action", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.MediaAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve media trails by username.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.MediaAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve media audit trails by user role.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.MediaAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve media audit trails within a date range.", ex);
            }
        }
    }
}
