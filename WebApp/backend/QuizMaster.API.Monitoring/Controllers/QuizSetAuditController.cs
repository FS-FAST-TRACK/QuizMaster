using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.QuizAudit;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizSetAuditController : ControllerBase
    {
        private readonly IQuizSetAuditTrailService _quizSetAuditTrailService;
        public QuizSetAuditController(IQuizSetAuditTrailService quizSetAuditTrailService)
        {
            _quizSetAuditTrailService = quizSetAuditTrailService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuizSetAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _quizSetAuditTrailService.GetAllQuizSetAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizSetAuditTrailByIdAsync(int id)
        {
            try
            {
                var auditTrail = await _quizSetAuditTrailService.GetQuizSetAuditTrailByIdAsync(id);
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
        public async Task<IActionResult> GetQuizSetAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _quizSetAuditTrailService.GetQuizSetAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }
        [HttpGet("username")]
        public async Task<IActionResult> GetQuizSetAuditTrailsByUsernameAsync([FromQuery] string username = "")
        {
            try
            {
                var auditTrails = await _quizSetAuditTrailService.GetQuizSetAuditTrailsByUsernameAsync(username);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }
        [HttpGet("userrole")]
        public async Task<IActionResult> GetQuizSetAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                var auditTrails = await _quizSetAuditTrailService.GetQuizSetAuditTrailsByUserRoleAsync(userRole);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by userrole: {ex.Message}");
            }
        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetQuizSetAuditTrailsWithinDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _quizSetAuditTrailService.GetQuizSetAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
