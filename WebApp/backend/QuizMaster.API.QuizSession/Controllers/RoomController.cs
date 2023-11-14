using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.QuizSession.Models;

namespace QuizMaster.API.QuizSession.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        [HttpPost]
        [Route("join/{roomId}")]
        public IActionResult JoinRoom(int roomId)
        {
            // clientId must be in the header
            // check the username in the JWT token
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{roomId}")]
        public IActionResult Submit(int roomId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{roomId}/chat")]
        public IActionResult PostChat(ChatPost post)
        {
            throw new NotImplementedException();
        }
    }
}
