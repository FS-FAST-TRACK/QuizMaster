using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Account.Api.Models
{
	public class UserForRegisterDto
	{

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }


		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }


		[Required]
		[MaxLength(50)]
		public string EmailAddress { get; set; }


		[Required]
		[MaxLength(50)]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }

	}
}
