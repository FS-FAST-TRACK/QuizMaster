using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Roles;

namespace QuizMaster.API.Authentication.Models
{
    public class AuthStore
    {
        public UserAccount UserData { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public AuthStore(UserAccount userData, IEnumerable<string> roles, DateTime timestamp, int validityOffsetHours)
        {
            UserData = userData;
            IssuedDate = timestamp;
            Roles = roles;
            ValidUntil = timestamp.AddHours(validityOffsetHours);
        }

        public bool IsExpired()
        {
            return ValidUntil < DateTime.Now;
        }
    }
}
