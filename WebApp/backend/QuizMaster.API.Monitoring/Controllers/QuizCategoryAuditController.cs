using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.QuizAudit;
using QuizMaster.Library.Common.Entities.Audits;
using System;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizCategoryAuditController : ControllerBase
    {
        private readonly IQuizCategoryAuditTrailService _quizCategoryAuditTrailService;
        public QuizCategoryAuditController(IQuizCategoryAuditTrailService quizCategoryAuditTrailService)
        {
            _quizCategoryAuditTrailService = quizCategoryAuditTrailService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllQuizCategoryAuditTrailsAsync()
        {
            try 
            {
                var auditTrails = await _quizCategoryAuditTrailService.GetAllQuizCategoryAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }
        [HttpGet("{quizAuditTrailId}")]
        public async Task<IActionResult> GetQuizCategoryAuditTrailByIdAsync(int quizAuditTrailId)
        {
            try 
            {
                var auditTrail = await _quizCategoryAuditTrailService.GetQuizCategoryAuditTrailByIdAsync(quizAuditTrailId);
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
        public async Task<IActionResult> GetQuizCategoryAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _quizCategoryAuditTrailService.GetQuizCategoryAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }
        [HttpGet("username")]
        public async Task<IActionResult> GetQuizCategoryAuditTrailsByUsernameAsync([FromQuery] string username = "")
        {
            try
            {
                var auditTrails = await _quizCategoryAuditTrailService.GetQuizCategoryAuditTrailsByUsernameAsync(username);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }



        [HttpGet("userrole")]
        public async Task<IActionResult> GetQuizCategoryAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                var auditTrails = await _quizCategoryAuditTrailService.GetQuizCategoryAuditTrailsByUserRoleAsync(userRole);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }

        }
        [HttpGet("date-range")]
        public async Task<IActionResult> GetQuizCategoryAuditTrailsWithinDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _quizCategoryAuditTrailService.GetQuizCategoryAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
