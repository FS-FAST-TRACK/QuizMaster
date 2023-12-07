using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.QuizAudit;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionTypeAuditController : ControllerBase
    {
        private readonly IQuestionTypeAuditTrailService _questionTypeAuditTrailService;

        public QuestionTypeAuditController(IQuestionTypeAuditTrailService questionTypeAuditTrailService)
        {
            _questionTypeAuditTrailService = questionTypeAuditTrailService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuestionTypeAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _questionTypeAuditTrailService.GetAllQuestionTypeAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionTypeAuditTrailByIdAsync(int id)
        {
            try
            {
                var auditTrail = await _questionTypeAuditTrailService.GetQuestionTypeAuditTrailByIdAsync(id);
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
        public async Task<IActionResult> GetQuestionTypeAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _questionTypeAuditTrailService.GetQuestionTypeAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }
        [HttpGet("username")]
        public async Task<IActionResult> GetQuestionTypeAuditTrailsByUsernameAsync([FromQuery] string username = "")
        {
            try
            {
                var auditTrails = await _questionTypeAuditTrailService.GetQuestionTypeAuditTrailsByUsernameAsync(username);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }
        [HttpGet("userrole")]
        public async Task<IActionResult> GetQuestionTypeAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                var auditTrails = await _questionTypeAuditTrailService.GetQuestionTypeAuditTrailsByUserRoleAsync(userRole);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetQuestionTypeAuditTrailsWithinDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _questionTypeAuditTrailService.GetQuestionTypeAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
