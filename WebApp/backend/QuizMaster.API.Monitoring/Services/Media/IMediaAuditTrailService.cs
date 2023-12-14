using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Services.Media
{
    public interface IMediaAuditTrailService
    {
        Task<IEnumerable<MediaAuditTrail>> GetAllMediaAuditTrailsAsync();
        Task<MediaAuditTrail> GetMediaAuditTrailByIdAsync(int id);
        Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByActionAsync(string action);
        Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByUsernameAsync(string username);
        Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsByUserRoleAsync(string userRole);
        Task<IEnumerable<MediaAuditTrail>> GetMediaAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
