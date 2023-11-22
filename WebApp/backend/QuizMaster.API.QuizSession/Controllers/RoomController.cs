using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuizMaster.API.QuizSession.Handlers;
using QuizMaster.API.QuizSession.Hubs;
using QuizMaster.API.QuizSession.Models;
using System.Reflection;

namespace QuizMaster.API.QuizSession.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IHubContext<SignalR_QuizSessionHub> _sessionHubContext;
        private readonly SessionHandler _sessionHandler;

        public RoomController(IHubContext<SignalR_QuizSessionHub> hubContext, SessionHandler sessionHandler)
        {
            _sessionHubContext = hubContext;
            _sessionHandler = sessionHandler;
        }

        [HttpPost]
        [Route("join/{roomId}")]
        public async Task<IActionResult> JoinRoom(int roomId, [FromBody] JoinData joinData)
        {
            // clientConnectionId must be in the header
            // TODO: check the username in the JWT token

            // TODO: ensure that the room exists, otherwise, return bad request

            // the groupName will be the roomID
            string groupName = $"room-{roomId}";
            // join the user in a specific room
            await _sessionHubContext.Groups.AddToGroupAsync(joinData.ConnectionId, groupName);
            // notify all players in a group that a user has joined
            await _sessionHubContext.Clients.Group(groupName).SendAsync("notif", new Notification { Type = "PlayerJoin", Message = $"{joinData.DisplayName} has joined the session" });

            // TODO: Add the participant in the list
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
        public async Task<IActionResult> PostChat(ChatPost post)
        {
            // TODO: ensure that the room exists

            string username = "FROMJWT.USERNAME";

            string groupName = $"room-{post.RoomId}";

            // generate a chat object
            Chat chat = new() { Sender = username, Message = post.Message };

            // save the chat in the handler
            _sessionHandler.ChatHandler.SaveChat(chat);

            // notify all users in the group that a chat has been posted
            await _sessionHubContext.Clients.Group(groupName).SendAsync("chat", _sessionHandler.ChatHandler.GetLatest());

            return Ok();
        }
    }
}
