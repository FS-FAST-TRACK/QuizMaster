using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Rooms
{
    public class QuizQuestion: IEntity
    {
        [Key]
        public int QQId { get; set; }

        [Required]
        public QuizRoom QRoom { get; set; }

        [Required]
        public Question Question { get; set; }

        [Required]
        public bool ActiveData { get; set; } = true;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [AllowNull]
        public DateTime? DateUpdated { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public int? UpdatedByUserId { get; set; }
    }
}
