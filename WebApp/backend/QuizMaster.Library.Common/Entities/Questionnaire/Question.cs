using Newtonsoft.Json;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
    public class Question: IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string QStatement { get; set; }

        [Required]
        [JsonIgnore]
        public IEnumerable<QuestionDetail> Details { get; set; }

        [Required]
        public string QImage { get; set; }

        [AllowNull]
        public string QAudio { get; set; }

        [Required] 
        public int QTime { get; set; }

        [Required]
        [ForeignKey(nameof(QDifficulty))]
        public int QDifficultyId { get; set; }
        public QuestionDifficulty QDifficulty { get; set; }


		[Required]
		[ForeignKey(nameof(QCategory))]
		public int QCategoryId { get; set; }
		public QuestionCategory QCategory { get; set; }

        [Required]
		[ForeignKey(nameof(QType))]
		public int QTypeId { get; set; }
		public QuestionType QType { get; set; }

        [Required]
        public bool ActiveData { get; set; } = true;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [AllowNull]
        public DateTime DateUpdated { get; set; }

		[Required]
		public int CreatedByUserId { get; set; }

		public int? UpdatedByUserId { get; set; }
	}
}
