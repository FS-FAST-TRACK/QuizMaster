using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class DetailDto
	{
		public int Id { get; set; }

		[Required]
		public string DetailDesc { get; set; }
	}
}
