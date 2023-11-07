using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDto
	{
		public int Id { get; set; }
		public string QStatement { get; set; }
		public string QAnswer { get; set; }
		public string QImage { get; set; }
		public string QAudio { get; set; }
		public int QTime { get; set; }
		public DifficultyDto QDifficulty { get; set; }
		public CategoryDto QCategory { get; set; }
		public TypeDto QType { get; set; }
		public DetailDto? Details { get; set; }
	}
}
