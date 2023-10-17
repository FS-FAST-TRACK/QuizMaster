using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Models.Account
{
	public class AccountCreatePartialDto
	{
		
		public string Email { get; set; }

		[Required]
		[MaxLength(20)]
		public string UserName { get; set; }

	}
}
