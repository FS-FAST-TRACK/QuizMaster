using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
    public class Question: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string QStatement { get; set; }

        [Required]
        public string QAnswer { get; set; }

        [Required]
        public string QImage { get; set; }

        [AllowNull]
        public string QAudio { get; set; }

        [Required]
        public QuestionDifficulty QDifficulty { get; set; }

        [Required]
        public QuestionCategory QCategory { get; set; }

        [Required]
        public QuestionType QType { get; set; }

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
