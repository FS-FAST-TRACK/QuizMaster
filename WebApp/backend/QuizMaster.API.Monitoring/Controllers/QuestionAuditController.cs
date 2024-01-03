using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.QuizAudit;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAuditController : ControllerBase
    {
        private readonly IQuestionAuditTrailService _questionAuditTrailService;
        public QuestionAuditController(IQuestionAuditTrailService questionAuditTrailService)
        {
            _questionAuditTrailService = questionAuditTrailService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuestionAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _questionAuditTrailService.GetAllQuestionAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionAuditTrailByIdAsync(int id)
        {
            try
            {
                var auditTrail = await _questionAuditTrailService.GetQuestionAuditTrailByIdAsync(id);
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
        public async Task<IActionResult> GetQuestionAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _questionAuditTrailService.GetQuestionAuditTrailsByActionAsync(action);
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
                var auditTrails = await _questionAuditTrailService.GetQuestionAuditTrailsByUsernameAsync(username);
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
                var auditTrails = await _questionAuditTrailService.GetQuestionAuditTrailsByUserRoleAsync(userRole);
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
                var auditTrails = await _questionAuditTrailService.GetQuestionAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
