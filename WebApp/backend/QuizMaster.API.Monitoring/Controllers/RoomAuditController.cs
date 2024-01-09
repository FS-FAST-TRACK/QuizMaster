using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Monitoring.Services.Room;

namespace QuizMaster.API.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomAuditController : ControllerBase
    {
        private readonly IRoomAuditTrailService _roomAuditTrailService;

        public RoomAuditController(IRoomAuditTrailService roomAuditTrailService)
        {
            _roomAuditTrailService = roomAuditTrailService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoomAuditTrailsAsync()
        {
            try
            {
                var auditTrails = await _roomAuditTrailService.GetAllRoomAuditTrailsAsync();
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve all audit trails: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomAuditTrailByIdAsync(int id)
        {
            try
            {
                var auditTrail = await _roomAuditTrailService.GetRoomAuditTrailByIdAsync(id);
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
        public async Task<IActionResult> GetRoomAuditTrailsByActionAsync([FromQuery] string action = "")
        {
            try
            {
                var auditTrails = await _roomAuditTrailService.GetRoomAuditTrailsByActionAsync(action);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by action: {ex.Message}");
            }
        }
        [HttpGet("username")]
        public async Task<IActionResult> GetRoomAuditTrailsByUsernameAsync([FromQuery] string username = "")
        {
            try
            {
                var auditTrails = await _roomAuditTrailService.GetRoomAuditTrailsByUsernameAsync(username);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }
        }

        [HttpGet("userrole")]
        public async Task<IActionResult> GetRoomAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                var auditTrails = await _roomAuditTrailService.GetRoomAuditTrailsByUserRoleAsync(userRole);
                return Ok(auditTrails);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return StatusCode(500, $"Failed to retrieve audit trails by username: {ex.Message}");
            }

        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetRoomAuditTrailsWithinDateRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var auditTrails = await _roomAuditTrailService.GetRoomAuditTrailsWithinDateRangeAsync(startDate, endDate);
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
