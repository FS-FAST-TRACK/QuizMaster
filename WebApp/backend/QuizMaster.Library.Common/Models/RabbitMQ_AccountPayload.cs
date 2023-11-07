using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.Library.Common.Models
{
    public class RabbitMQ_AccountPayload
    {
        public UserAccount Account { get; set; }
        public IList<string> Roles { get; set; }
    }
}
