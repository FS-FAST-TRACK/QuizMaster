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
	public class Detail : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string DetailDesc { get; set; }
		public bool ActiveData { get ; set ; }
		public DateTime DateCreated { get ; set ; }
		public DateTime DateUpdated { get ; set ; }
		public int CreatedByUserId { get ; set ; }
		public int? UpdatedByUserId { get ; set ; }
	}
}
