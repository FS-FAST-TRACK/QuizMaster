using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Utilities;

namespace QuizMaster.API.Authentication.Services.Temp
{
    public class Repository : IRepository
    {
        private readonly IEnumerable<UserAccount> _users;

        public Repository()
        {
            _users = new List<UserAccount>()
            {
                // create dummy account
                new UserAccount
                {
                    Id = 1,
                    Lastname = "Joe",
                    Firstname = "Nax",
                    Email = "naxjoe@email.com",
                    Username = "naxjoe",
                    Password = Hash.GetHashSha256("naxjoe"),
                    UserRole = Library.Common.Entities.Details.UserRole.USER,
                    ActiveData = true,
                    DateCreated = DateTime.UtcNow,
                },
                new UserAccount
                {
                    Id = 1,
                    Lastname = "[Admin] Joe",
                    Firstname = "Nax",
                    Email = "naxjoeadmin@email.com",
                    Username = "naxjoeadmin",
                    Password = Hash.GetHashSha256("naxjoeadmin"),
                    UserRole = Library.Common.Entities.Details.UserRole.ADMIN,
                    ActiveData = true,
                    DateCreated = DateTime.UtcNow,
                },
            };
        }
        public UserAccount GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email) ?? new() { Id = -1};
        }

        public UserAccount GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username) ?? new() { Id = -1 };
        }
    }
}
