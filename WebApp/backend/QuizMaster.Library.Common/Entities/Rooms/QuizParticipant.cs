using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Rooms
{
    public class QuizParticipant : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } = new Random().Next(1000000,9999999);

		[Required]
		[MaxLength(50)]
		public string QParticipantDesc { get; set; }

		[AllowNull]
		public int QRoomId { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		public int Score { get; set; }

		[Required]
		public DateTime? QStartDate { get; set; } = null;

		[AllowNull]
		public DateTime? QEndDate { get; set; } = null;

		[Required]
		public bool QStatus { get; set; } = false;

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
