using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class TypeDto
	{
		public int Id { get; set; }

		public string QTypeDesc { get; set; }

		public bool QDetailRequired { get; set; } = false;

	}
}
