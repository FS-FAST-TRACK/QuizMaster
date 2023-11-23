using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.Library.Common.Entities.Audits;

namespace MonitoringService.DataAccess
{
    public class UserAuditTrailRepository : IUserAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;

        public UserAuditTrailRepository(MonitoringDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAllUserAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.UserAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                // You can log the exception using a logging framework like Serilog, NLog, or others
                throw new Exception("Failed to retrieve audit trails.", ex);
            }
        }

        public async Task<UserAuditTrail> GetUserAuditTrailByIdAsync(int auditTrailId)
        {
            try
            {
                return await _dbContext.UserAuditTrails.FirstOrDefaultAsync(a => a.UserAuditTrailId == auditTrailId);
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task AddAuditTrailAsync(UserAuditTrail auditTrail)
        {
            try
            {
                _dbContext.UserAuditTrails.Add(auditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to add audit trail.", ex);
            }
        }



        public async Task DeleteAuditTrailAsync(int auditTrailId)
        {
            try
            {
                var auditTrail = await _dbContext.UserAuditTrails.FirstOrDefaultAsync(a => a.UserAuditTrailId == auditTrailId);
                if (auditTrail != null)
                {
                    _dbContext.UserAuditTrails.Remove(auditTrail);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to delete audit trail.", ex);
            }
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAuditTrailsForUserAsync(int userId)
        {
            try
            {
                return await _dbContext.UserAuditTrails.Where(a => a.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve audit trails for a user.", ex);
            }
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAuditTrailsByActionAsync(string action)
        {
            try
            {
                // Use Equals for exact match
                return await _dbContext.UserAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.UserAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }

    }
}
