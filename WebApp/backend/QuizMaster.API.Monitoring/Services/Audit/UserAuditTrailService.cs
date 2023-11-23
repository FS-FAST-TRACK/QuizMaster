using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.Audit
{
    public class UserAuditTrailService : IUserAuditTrailService
    {
        private readonly IUserAuditTrailRepository _auditTrailRepository;

        public UserAuditTrailService(IUserAuditTrailRepository auditTrailRepository)
        {
            _auditTrailRepository = auditTrailRepository;
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAllUserAuditTrailsAsync()
        {
            try
            {
                return await _auditTrailRepository.GetAllUserAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trails.", ex);
            }
        }

        public async Task<UserAuditTrail> GetUserAuditTrailByIdAsync(int auditTrailId)
        {
            try
            {
                return await _auditTrailRepository.GetUserAuditTrailByIdAsync(auditTrailId);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task AddAuditTrailAsync(UserAuditTrail auditTrail)
        {
            try
            {
                await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to add audit trail.", ex);
            }
        }

        public async Task DeleteAuditTrailAsync(int auditTrailId)
        {
            try
            {
                await _auditTrailRepository.DeleteAuditTrailAsync(auditTrailId);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to delete audit trail.", ex);
            }
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAuditTrailsForUserAsync(int userId)
        {
            try
            {
                return await _auditTrailRepository.GetAuditTrailsForUserAsync(userId);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails for a user.", ex);
            }
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _auditTrailRepository.GetAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }

        public async Task<IEnumerable<UserAuditTrail>> GetAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _auditTrailRepository.GetAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
