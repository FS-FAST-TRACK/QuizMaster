using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Questionnaire
{
	[PrimaryKey(nameof(QuestionDetailId), nameof(DetailTypeId))]
	public class QuestionDetailType : IEntity
    {
       
		[ForeignKey(nameof(QuestionDetail))]
		public int QuestionDetailId { get; set; }
		public QuestionDetail QuestionDetail { get; set; }

		[ForeignKey(nameof(DetailType))]
		public int DetailTypeId { get; set; }
		public DetailType DetailType { get; set; }

		public bool ActiveData { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public int CreatedByUserId { get; set; } = 1; // Admin as default creator
        public int? UpdatedByUserId { get; set; }
    }
}
