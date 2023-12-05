using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class CategoryDto
	{
		public int Id { get; set; }
		public string QCategoryDesc { get; set; }
		public int QuestionCounts { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateUpdated { get; set; }
	}
}
