using QuizMaster.API.QuizSession.Models;

namespace QuizMaster.API.QuizSession.Services.Repositories
{
    public interface IChatRepository
    {
        public Chat? GetChat(Guid id);
        public IReadOnlyList<Chat> GetChats();
        public void SaveChat(Chat chat);
        public bool DeleteChat(Guid id);
        public Chat? GetLatest();
    }
}
