using QuizMaster.API.QuizSession.Services.Repositories;

namespace QuizMaster.API.QuizSession.Handlers
{
    public class SessionHandler
    {
        /*
         * This handler is responsible for adding rooms, saving chats
         * updating room information, soft deleting rooms, etc.
         */

        private readonly IChatRepository _chats;

        public SessionHandler(IChatRepository chats)
        {
            _chats = chats;
        }

        public IChatRepository ChatHandler { get { return _chats; } }
    }
}
