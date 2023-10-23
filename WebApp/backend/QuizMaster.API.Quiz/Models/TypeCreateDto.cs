using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class TypeCreateDto
	{
		[Required]
		[MaxLength(50)]
		public string QTypeDesc { get; set; }

		public bool QDetailRequired { get; set; } = false;
	}
}
