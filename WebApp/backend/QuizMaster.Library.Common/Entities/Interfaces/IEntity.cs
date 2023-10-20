using QuizMaster.Library.Common.Entities.Accounts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Interfaces
{
    public interface IEntity
    {
        bool ActiveData { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
		public int CreatedByUserId { get; set; }
		public int? UpdatedByUserId { get; set; }
	}
}
