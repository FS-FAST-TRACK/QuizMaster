using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Common.Library.Entities
{
	public class Question : IEntity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string QStatement { get; set; }

		[Required] 
		public string QAnswer { get; set; }

		[Required] 
		public string QImage { get; set; }

		public string QAudio { get; set; }

		public QuestionDifficulty QDifficulty { get; set; }

		public QuestionCategory QCategory { get; set; }

		public QuestionType QType { get; set; }

		public Boolean ActiveData { get; set; } = true;


		[Required]
		public DateTime DateCreated { get; set; }

		public DateTime DateUpdated { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public UserAccount User { get; set; }
	}
}
