using QuizMaster.Common.Library.Roles;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Common.Library.Entities
{
	public class UserAccount : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

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

		[ForeignKey("UserRole")]
		public int UserRoleId { get; set; }= RolesData.User.Id; // User Role is set to default User
		public UserRole UserRole { get; set; } 

		public Boolean ActiveData { get; set; } = true; // Active data is set to default true

		[Required]
		public DateTime DateCreated { get; set; }

		public DateTime DateUpdated { get; set; }

		[ForeignKey("User")]
		public int? UpdatedByUserId { get; set; }
		public UserAccount UpdatedByUser { get; set; }


	}
}
