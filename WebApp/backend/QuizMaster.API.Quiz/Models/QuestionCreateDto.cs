using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class QuestionCreateDto
	{
		[Required]
		public string QStatement { get; set; }

		[Required]
		public string QAnswer { get; set; }

		[Required]
		public string QImage { get; set; }

		[AllowNull]
		public string QAudio { get; set; }

		[Required]
		public int QDifficultyId { get; set; }

		[Required]
		public int QCategoryId { get; set; }

		[Required]
		public int QTypeId { get; set; }

		// QDetails is a json serialized format of the type of details accepted by a question type
		public string? QDetails { get; set; }
	}
}
