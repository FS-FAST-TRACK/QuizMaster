using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
    public class QuestionCategory: IEntity
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int  Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string QCategoryDesc { get; set; }

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
