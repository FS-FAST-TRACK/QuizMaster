using System.ComponentModel.DataAnnotations;

namespace QuizMaster.Common.Library.Entities
{
	public class UserRole 
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(15)]
		public string UserRoleDesc { get; set; }
	}
}
