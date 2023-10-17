using Microsoft.AspNetCore.Identity;
using QuizMaster.Library.Common.Entities.Details;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Accounts
{
    public class UserAccount: IdentityUser<int>
    {
        [Key]
        public override int Id { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public override string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public override string UserName { get; set; }

        [Required] 
        public bool ActiveData { get; set; } = true;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;


        public DateTime? DateUpdated { get; set; }

        public UserAccount? UpdatedByUser { get; set; }

    }
}
