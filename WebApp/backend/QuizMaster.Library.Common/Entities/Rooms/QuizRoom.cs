using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Rooms
{
    public class QuizRoom: IEntity
    {
        [Key]
        public int QRoomId { get; set; }

        [Required]
        public string QRoomDesc { get; set; }

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
