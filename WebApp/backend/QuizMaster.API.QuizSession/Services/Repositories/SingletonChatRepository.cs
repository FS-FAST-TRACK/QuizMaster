using QuizMaster.API.QuizSession.Models;

namespace QuizMaster.API.QuizSession.Services.Repositories
{
    public class SingletonChatRepository : IChatRepository
    {
        private readonly List<Chat> _chats;
        private Chat? latest;

        public SingletonChatRepository()
        {
            _chats = new();
            latest = null;
        }

        public bool DeleteChat(Guid id)
        {
            var chat = GetChat(id);
            if(chat != null)
            {
                _chats.Remove(chat);
                return true;
            }
            return false;
        }

        public Chat? GetChat(Guid id)
        {
            return _chats.FirstOrDefault(c => c.Id == id);
        }

        public IReadOnlyList<Chat> GetChats()
        {
            return _chats.AsReadOnly();
        }

        public Chat? GetLatest()
        {
            return latest;
        }

        public void SaveChat(Chat chat)
        {
            _chats.Add(chat);
            latest = chat;
        }
    }
}
