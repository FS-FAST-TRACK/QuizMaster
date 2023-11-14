using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class OptionCreateDto
	{
		[Required]
		public string Value { get; set; }
		public bool IsAnswer { get; set; } = false;

	}
}
