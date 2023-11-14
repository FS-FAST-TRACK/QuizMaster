using Microsoft.AspNetCore.SignalR;
using QuizMaster.API.QuizSession.Handlers;
using QuizMaster.API.QuizSession.Models;

namespace QuizMaster.API.QuizSession.Hubs
{
    public class SignalR_QuizSessionHub : Hub
    {
        /*
         * This hub is responsible for handling session in real-time such as
         * chat updates, room information, quiz started, quiz ended, quiz delete,
         * player join, etc.
         */

        private readonly SessionHandler sessionHandler;

        public SignalR_QuizSessionHub(SessionHandler sessionHandler) 
        {
            this.sessionHandler = sessionHandler;
        }

        // on player/user join
        public override async Task OnConnectedAsync()
        {
            var clientConnectionId = Context.ConnectionId;

            // send the user it's connection ID to use the api/room/join/{roomId} route which required connectionId
            await Clients.Client(clientConnectionId).SendAsync("notif", new Notification { Type = "PlayerInfo", Message = clientConnectionId });
        }


        // on player/user left
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var clientConnectionId = Context.ConnectionId;

            // retrieve the clientConnectionId from participants list then send it to notif
            // must check if what group does the user belong and send a notif there
            await Clients.All.SendAsync("notif", new Notification { Type = "PlayerLeave", Message = "A player has left" });
        }

    }
}
