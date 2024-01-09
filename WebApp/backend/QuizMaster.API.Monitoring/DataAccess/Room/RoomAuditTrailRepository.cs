using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess.Room
{
    public class RoomAuditTrailRepository : IRoomAuditTrailRepository
    {
        private readonly MonitoringDbContext _dbContext;
        public RoomAuditTrailRepository(MonitoringDbContext dbContext)
        {

            _dbContext = dbContext;

        }
        public async Task AddRoomAuditTrailAsync(RoomAuditTrail roomAuditTrail)
        {
            try
            {
                _dbContext.Add(roomAuditTrail);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add room audit trail.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetAllRoomAuditTrailsAsync()
        {
            try
            {
                return await _dbContext.RoomAuditTrails.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve room audit trails.", ex);
            }
        }

        public async Task<RoomAuditTrail> GetRoomAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _dbContext.RoomAuditTrails.FirstOrDefaultAsync(a => a.RoomAuditTrailId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve room audit trails by ID.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByActionAsync(string action)
        {
            try
            {
                //Use Equals for exact match
                return await _dbContext.RoomAuditTrails
                    .Where(a => a.Action.Equals(action))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrive quiz set audit trails by action", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _dbContext.RoomAuditTrails
                    .Where(a => a.UserName.Equals(username))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve quizset audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _dbContext.RoomAuditTrails
                    .Where(a => a.UserRole.Equals(userRole))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve room audit trails by user role.", ex);
            }
        }

        public async Task<IEnumerable<RoomAuditTrail>> GetRoomAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _dbContext.RoomAuditTrails.Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response
                throw new Exception("Failed to retrieve room audit trails within a date range.", ex);
            }
        }
    }
}
