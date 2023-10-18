using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Roles
{
	public class UserRole : IdentityRole<int>
	{
		[Key]
		public override int Id { get; set; }
		[Required]
		[MaxLength(15)]
		public string UserRoleDesc { get; set; }
	}
}
