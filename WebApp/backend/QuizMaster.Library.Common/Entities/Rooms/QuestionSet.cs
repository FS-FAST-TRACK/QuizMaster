using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Library.Common.Entities.Rooms
{
	[PrimaryKey(nameof(QuestionId), nameof(SetId))]
	public class QuestionSet : IEntity
	{
		[ForeignKey(nameof(Question))]
		public int QuestionId { get; set; }
		public Question Question { get; set; }

		[ForeignKey(nameof(Set))]
		public int SetId { get; set; }
		public Set Set { get; set; }

		public bool ActiveData { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
		public int CreatedByUserId { get; set; }
		public int? UpdatedByUserId { get; set; }
	}
}
