using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
    public class QuestionDifficulty : IEntity
    {
        [Key]
        public int QDifficultyId { get; set; }

        [Required]
        [MaxLength(10)]
        public string QDifficultyDesc { get; set; }

        [Required] 
        public bool ActiveData { get; set; } = true;
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [AllowNull]
        public DateTime DateUpdated { get; set; }
        [Required]
        public UserAccount CreatedByUser { get; set; }
        [AllowNull]
        public UserAccount UpdatedByUser { get; set; }
    }
}
