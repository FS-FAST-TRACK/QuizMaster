using QuizMaster.Library.Common.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Models.Account
{
	public class AccountDto
	{
		public int Id { get; set; }

		[MaxLength(50)]
		public string? LastName { get; set; }

		[MaxLength(50)]
		public string? FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string Email { get; set; }

		[Required]
		[MaxLength(20)]
		public string UserName { get; set; }

		[Required]
		public bool ActiveData { get; set; } = true;

		[Required]
		public DateTime DateCreated { get; set; } = DateTime.UtcNow;


		public DateTime? DateUpdated { get; set; }

		public UserAccount? UpdatedByUser { get; set; }
	}
}
