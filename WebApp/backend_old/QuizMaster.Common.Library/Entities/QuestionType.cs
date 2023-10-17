using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;


namespace QuizMaster.Common.Library.Entities
{
	public class QuestionType : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string QTypeDesc { get; set; }

		public Boolean QDetailRequired { get; set; } = false;

		public Boolean ActiveData { get; set; } = true;

		[Required]
		public DateTime DateCreated { get; set; }

		public DateTime DateUpdated { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public UserAccount User { get; set; }
	}
}
