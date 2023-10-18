using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Models.Account
{
	public class AccountCreateDto
	{

		[MaxLength(50)]
		public string LastName { get; set; }

		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string Email { get; set; }

		[Required]
		[MaxLength(20)]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
