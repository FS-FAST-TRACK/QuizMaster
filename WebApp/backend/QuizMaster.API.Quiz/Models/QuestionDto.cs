using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDto
	{
		public string QStatement { get; set; }
		public string QAnswer { get; set; }
		public string QImage { get; set; }
		public string QAudio { get; set; }
		public QuestionDifficulty QDifficulty { get; set; }
		public QuestionCategory QCategory { get; set; }
		public QuestionType QType { get; set; }
		public string? Details { get; set; }
	}
}
