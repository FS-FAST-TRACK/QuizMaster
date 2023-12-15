using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.Media;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaAuditController : ControllerBase
    {
        private readonly IMediaAuditTrailService _mediaAuditTrailService;

        public MediaAuditController(IMediaAuditTrailService mediaAuditTrailService)
        {
            _mediaAuditTrailService = mediaAuditTrailService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMediaAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _mediaAuditTrailService.GetAllMediaAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMediaAuditTrailByIdAsync(int id)
        {
            try
            {
                var auditTrail = await _mediaAuditTrailService.GetMediaAuditTrailByIdAsync(id);
                if (auditTrail == null)
                {
                    return NotFound($"Audit trail with ID {id} not found.");
                }
                return Ok(auditTrail);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trail by ID: {ex.Message}");
            }
        }
        [HttpGet("action")]
        public async Task<IActionResult> GetMediaAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _mediaAuditTrailService.GetMediaAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }
        [HttpGet("username")]
        public async Task<IActionResult> GetMediaAuditTrailsByUsernameAsync([FromQuery] string username = "")
        {
            try
            {
                var auditTrails = await _mediaAuditTrailService.GetMediaAuditTrailsByUsernameAsync(username);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }
        [HttpGet("userrole")]
        public async Task<IActionResult> GetMediaAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                var auditTrails = await _mediaAuditTrailService.GetMediaAuditTrailsByUserRoleAsync(userRole);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }

        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetMediaAuditTrailsWithinDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _mediaAuditTrailService.GetMediaAuditTrailsWithinDateRangeAsync(startDate, endDate);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails within date range: {ex.Message}");
            }
        }
    }
}
