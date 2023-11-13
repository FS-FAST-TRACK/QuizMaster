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
    public class QuestionDetailType : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        public string DTypeDesc { get; set; }
        public bool ActiveData { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public int CreatedByUserId { get; set; } = 1; // Admin as default creator
        public int? UpdatedByUserId { get; set; }
    }
}
