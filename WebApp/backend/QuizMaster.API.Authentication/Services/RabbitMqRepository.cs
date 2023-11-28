using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models;
using QuizMaster.Library.Common.Models.Services;

namespace QuizMaster.API.Authentication.Services
{
    public class RabbitMqRepository
    {
        private readonly Dictionary<int, RabbitMQ_AccountPayload> users;
        private readonly ILogger<RabbitMqRepository> logger;

        public RabbitMqRepository(ILogger<RabbitMqRepository> logger) 
        { 
            users = new Dictionary<int, RabbitMQ_AccountPayload>();
            this.logger = logger;
        }



        public void AddCache(RabbitMQ_AccountPayload accountPayload)
        {
            /*
            if (!users.ContainsKey(account.Id))
            {
                logger.LogInformation("Cached Data for User: "+account.UserName);
                users.Add(account.Id, account);
            }
            */

            if(accountPayload.Account.Id != -1)
            {
                if(!users.ContainsKey(accountPayload.Account.Id)) 
                {
                    logger.LogInformation("Cached Data for User: " + accountPayload.Account.UserName);
                    users.Add(accountPayload.Account.Id, accountPayload);
                }
                else
                {
                    users[accountPayload.Account.Id] = accountPayload;
                }
            }
        }

        public RabbitMQ_AccountPayload TryGetCache(AuthRequest authRequest)
        {
            RabbitMQ_AccountPayload? accountPayload = null;
            foreach (var (k, v) in users)
            {
                if (v.Account == null)
                    continue;
                if (v.Account.UserName != null)
                    if (v.Account.UserName.ToLower() == authRequest.Username.ToLower())
                        accountPayload = v;
                if (v.Account.Email != null)
                    if (v.Account.Email.ToLower() == authRequest.Email.ToLower())
                        accountPayload = v;
                // try parsing the username to Id
                _ = Int32.TryParse(authRequest.Username, out int Id);
                if (Id != 0)
                    if (Id == k)
                        accountPayload = v;
            }

            if( accountPayload != null)
            {
                users.Remove(accountPayload.Account.Id);
                return accountPayload;
            }

            return new() { Account = new UserAccount { Id = -1 }, Roles = new List<string>() };
        }
    }
}
