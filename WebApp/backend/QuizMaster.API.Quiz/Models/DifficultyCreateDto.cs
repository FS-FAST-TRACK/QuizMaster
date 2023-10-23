using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class DifficultyCreateDto
	{
		[Required]
		[MaxLength(10)]
		public string QDifficultyDesc { get; set; }
}
}
