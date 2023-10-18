using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Roles;

namespace QuizMaster.API.Authentication.Services.Temp
{
    public interface IRepository
    {
        UserAccount GetUserByEmail(string email);
        UserAccount GetUserByUsername(string username);
        IEnumerable<UserRole> GetRoles(int userId);
    }
}
