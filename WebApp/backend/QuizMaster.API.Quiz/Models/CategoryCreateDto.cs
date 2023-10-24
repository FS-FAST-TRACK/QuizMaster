using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class CategoryCreateDto
	{
		[Required]
		[MaxLength(50)]
		public string QCategoryDesc { get; set; }
	}
}
