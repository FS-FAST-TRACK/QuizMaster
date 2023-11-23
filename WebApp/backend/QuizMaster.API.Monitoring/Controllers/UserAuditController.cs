using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.Audit;

namespace QuizMaster.API.Monitoring.Controllers
{
    [ApiController]
    [Route("api/audit/user")]
    public class UserAuditController : ControllerBase
    {
        private readonly IUserAuditTrailService _auditTrailService;

        public UserAuditController(IUserAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _auditTrailService.GetAllUserAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }

        [HttpGet("{auditTrailId}")]
        public async Task<IActionResult> GetAuditTrailByIdAsync(int auditTrailId)
        {
            try
            {
                var auditTrail = await _auditTrailService.GetUserAuditTrailByIdAsync(auditTrailId);
                if (auditTrail == null)
                {
                    return NotFound($"Audit trail with ID {auditTrailId} not found.");
                }
                return Ok(auditTrail);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trail by ID: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAuditTrailsForUser(int userId)
        {
            try
            {
                var auditTrails = await _auditTrailService.GetAuditTrailsForUserAsync(userId);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails for user: {ex.Message}");
            }
        }

        [HttpGet("action")]
        public async Task<IActionResult> GetAuditTrailsByAction([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _auditTrailService.GetAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetAuditTrailsWithinDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _auditTrailService.GetAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
