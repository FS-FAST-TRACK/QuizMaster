using QuizMaster.Library.Common.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Rooms
{
	public class Set : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string QSetName { get; set; }
		public string QSetDesc { get; set; }
		public bool ActiveData { get ; set ; }
		public DateTime DateCreated { get ; set ; }
		public DateTime DateUpdated { get ; set ; }
		public int CreatedByUserId { get ; set ; }
		public int? UpdatedByUserId { get ; set ; }
	}
}
