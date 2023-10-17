using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Common.Library.Entities
{
	public class QuestionDetail : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string QDetailDesc { get; set; }

		public Question Q { get; set; }

		public Boolean ActiveData { get; set; } = true;


		[Required]
		public DateTime DateCreated { get; set; }

		public DateTime DateUpdated { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public UserAccount User { get; set; }

	}
}
