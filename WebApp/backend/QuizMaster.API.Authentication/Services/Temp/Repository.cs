using Microsoft.AspNetCore.Identity;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Details;
using QuizMaster.Library.Common.Entities.Roles;
using QuizMaster.Library.Common.Utilities;

namespace QuizMaster.API.Authentication.Services.Temp
{
    public class Repository : IRepository
    {
        private readonly IEnumerable<UserAccount> _users;
        private readonly IEnumerable<UserRole> _roles;
        private readonly IEnumerable<IdentityUserRole<int>> _identityRoles;

        public Repository()
        {
            _roles = new List<UserRole>()
            {
                UserRoles.Admin,
                UserRoles.User
            };

            PasswordHasher<UserAccount> hasher = new PasswordHasher<UserAccount>();

            var userNax = new UserAccount
            {
                Id = 1,
                LastName = "Joe",
                FirstName = "Nax",
                Email = "naxjoe@email.com",
                UserName = "naxjoe",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var adminNax = new UserAccount
            {
                Id = 2,
                LastName = "[Admin] Joe",
                FirstName = "Nax",
                Email = "naxjoeadmin@email.com",
                UserName = "naxjoeadmin",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            userNax.PasswordHash = hasher.HashPassword(userNax, "naxjoe");
            adminNax.PasswordHash = hasher.HashPassword(adminNax, "naxjoeadmin");

            _users = new List<UserAccount>()
            {
                // create dummy account
                userNax,
                adminNax
            };

            _identityRoles = new List<IdentityUserRole<int>>()
            {
                new IdentityUserRole<int>{UserId = 1, RoleId = 2},
                new IdentityUserRole<int>{UserId = 2, RoleId = 1},
            };
        }

        public IEnumerable<UserRole> GetRoles(int userId)
        {
            var identityUserRoles = _identityRoles.Where(r => r.UserId ==  userId).ToList();

            List<UserRole> roles = new();
            identityUserRoles.ForEach(iUR =>
            {
                roles.Add(_roles.Where(r => r.Id == iUR.RoleId).First());
            });

            return roles;
        }

        public UserAccount GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email) ?? new() { Id = -1};
        }

        public UserAccount GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.UserName == username) ?? new() { Id = -1 };
        }
    }
}
