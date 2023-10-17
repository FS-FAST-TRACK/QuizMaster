using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Account.Api.Models
{
	public class UserDto
	{
		public int Id { get; set; }
		public string LastName { get; set; }

		public string FirstName { get; set; }

		public string EmailAddress { get; set; }

		public string UserName { get; set; }

		public int UserRoleId { get; set; }
	}
}
