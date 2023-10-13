using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Account.Api.Models
{
	public class UserForLoginDto
	{


		[Required]
		[MaxLength(50)]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }

		
	}
}
