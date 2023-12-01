using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.QuizAudit;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizDifficultyAuditController : ControllerBase
    {
        private readonly IQuizDifficultyAuditTrailService _quizDifficultyAuditTrailService;
        public QuizDifficultyAuditController(IQuizDifficultyAuditTrailService quizDifficultyAuditTrailService)
        {
            _quizDifficultyAuditTrailService = quizDifficultyAuditTrailService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuizDifficultyAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _quizDifficultyAuditTrailService.GetAllQuizDifficultyAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }
        [HttpGet("{quizAuditTrailId}")]
        public async Task<IActionResult> GetQuizDifficultyAuditTrailByIdAsync(int quizAuditTrailId)
        {
            try
            {
                var auditTrail = await _quizDifficultyAuditTrailService.GetQuizDifficultyAuditTrailByIdAsync(quizAuditTrailId);
                if (auditTrail == null)
                {
                    return NotFound($"Audit trail with ID {quizAuditTrailId} not found.");
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
        public async Task<IActionResult> GetQuizDifficultyAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _quizDifficultyAuditTrailService.GetQuizDifficultyAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }
        [HttpGet("username")]
        public async Task<IActionResult> GetQuizDifficultyAuditTrailsByUsernameAsync([FromQuery] string username = "")
        {
            try
            {
                var auditTrails = await _quizDifficultyAuditTrailService.GetQuizDifficultyAuditTrailsByUsernameAsync(username);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }



        [HttpGet("userrole")]
        public async Task<IActionResult> GetQuizDifficultyAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                var auditTrails = await _quizDifficultyAuditTrailService.GetQuizDifficultyAuditTrailsByUserRoleAsync(userRole);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }

        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetQuizDifficultyAuditTrailsWithinDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _quizDifficultyAuditTrailService.GetQuizDifficultyAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
