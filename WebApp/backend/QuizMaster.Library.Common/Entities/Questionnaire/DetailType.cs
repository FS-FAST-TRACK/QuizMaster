using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
	public class DetailType : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string DTypeDesc { get; set; }
		public bool ActiveData { get ; set ; }
		public DateTime DateCreated { get ; set ; }
		public DateTime DateUpdated { get ; set ; }
		public int CreatedByUserId { get ; set ; }
		public int? UpdatedByUserId { get ; set ; }
	}
}
