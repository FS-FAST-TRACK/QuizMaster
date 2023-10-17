using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.API.Authentication.Services.Temp
{
    public interface IRepository
    {
        UserAccount GetUserByEmail(string email);
        UserAccount GetUserByUsername(string username);
    }
}
