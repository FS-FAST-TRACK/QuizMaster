using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
    public class QuestionDetail: IEntity
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

		[ForeignKey(nameof(Detail))]
		public int DetailId { get; set; }
		public Detail Detail { get; set; }

		[ForeignKey(nameof(QuestionDetailType))]
		public int QuestionDetailTypeId { get; set; }
		public QuestionDetailType QuestionDetailType { get; set; }



		[Required]
        public bool ActiveData { get; set; } = true;

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [AllowNull]
        public DateTime DateUpdated { get; set; }

        [Required]
        public int CreatedByUserId { get; set; } = 1; 

		public int? UpdatedByUserId { get; set; }
	}
}
