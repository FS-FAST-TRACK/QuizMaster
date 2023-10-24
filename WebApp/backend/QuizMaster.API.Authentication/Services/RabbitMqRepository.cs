using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models.Services;

namespace QuizMaster.API.Authentication.Services
{
    public class RabbitMqRepository
    {
        private readonly Dictionary<int, UserAccount> users;
        private readonly ILogger<RabbitMqRepository> logger;

        public RabbitMqRepository(ILogger<RabbitMqRepository> logger) 
        { 
            users = new Dictionary<int, UserAccount>();
            this.logger = logger;
        }



        public void AddCache(UserAccount account)
        {
            if (!users.ContainsKey(account.Id))
            {
                logger.LogInformation("Cached Data for User: "+account.UserName);
                users.Add(account.Id, account);
            }
        }

        public UserAccount TryGetCache(AuthRequest authRequest)
        {
            foreach (var (k, v) in users)
            {
                if (v.UserName != null)
                    if (v.UserName.ToLower() == authRequest.Username.ToLower())
                        return v;
                if (v.Email != null)
                    if (v.Email.ToLower() == authRequest.Email.ToLower())
                        return v;
            }

            return new() { Id = -1 };
        }
    }
}
