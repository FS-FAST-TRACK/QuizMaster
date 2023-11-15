using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.QuizSession.Models;

namespace QuizMaster.API.QuizSession.Controllers
{
    // Only Admin has an access to this controller
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateSession(SessionDTO sessionDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Route("Update")]
        public IActionResult UpdateSession(SessionDTO sessionDTO)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteSession(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Start/{id}")]
        public IActionResult StartSession(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult GetSession(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult GetSessions()
        {
            throw new NotImplementedException();
        }
    }
}
