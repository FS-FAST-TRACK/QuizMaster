using QuizMaster.Library.Common.Entities.Details;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Accounts
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [AllowNull]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [AllowNull]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole UserRole { get; set; } = UserRole.USER;
        [Required] 
        public bool ActiveData { get; set; } = true;
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [AllowNull]
        public DateTime DateUpdated { get; set; }
        [AllowNull]
        public UserAccount UpdatedByUser { get; set; }

    }
}
