using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.Library.Common.Entities.Rooms
{
    public class QuizRoom: IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string QRoomDesc { get; set; }

        [Range(10000000, 99999999)]
        public int QRoomPin { get; set; }

        public string RoomOptions { get; set; } = string.Empty;

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
