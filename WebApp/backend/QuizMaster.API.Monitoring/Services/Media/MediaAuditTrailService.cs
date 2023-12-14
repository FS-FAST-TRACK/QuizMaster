using QuizMaster.API.Monitoring.DataAccess.Media;
using QuizMaster.Library.Common.Entities.Audits;
using System;

namespace QuizMaster.API.Monitoring.Services.Media
{
    public class MediaAuditTrailService : IMediaAuditTrailService
    {
        private readonly IMediaAuditTrailRepository _mediaAuditTrailRepository;

        public MediaAuditTrailService(IMediaAuditTrailRepository mediaAuditTrailRepository)
        {
            _mediaAuditTrailRepository = mediaAuditTrailRepository;
        }
        public async Task<IEnumerable<MediaAuditTrail>> GetAllMediaAuditTrailsAsync()
        {
            try
            {
                return await _mediaAuditTrailRepository.GetAllMediaAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve media audit trails.", ex);
            }
        }

        public async Task<MediaAuditTrail> GetMediaAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _mediaAuditTrailRepository.GetMediaAuditTrailByIdAsync(id);
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _mediaAuditTrailRepository.GetMediaAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trail by Action.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _mediaAuditTrailRepository.GetMediaAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trail by Username.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _mediaAuditTrailRepository.GetMediaAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trail by UserRole.", ex);
            }
        }

        public async Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _mediaAuditTrailRepository.GetMediaAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve audit trail by Date Range.", ex);
            }
        }
    }
}
