using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.Library.Common.Entities.Interfaces
{
    public interface IEntity
    {
        bool ActiveData { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        UserAccount CreatedByUser { get; set; }
        UserAccount UpdatedByUser { get; set; }
    }
}
