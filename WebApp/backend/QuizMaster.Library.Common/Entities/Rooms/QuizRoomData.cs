using Newtonsoft.Json;
using QuizMaster.Library.Common.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizMaster.Library.Common.Entities.Rooms
{
    public class QuizRoomData : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Quiz Room
        [ForeignKey(nameof(QuizRoom))]
        public int QRoomId { get; set; }

        // Set QuizRooms
        public string SetQuizRoomJSON { get; set; } = string.Empty;

        // Participants
        public string ParticipantsJSON { get; set; } = string.Empty;

        public DateTime StartedDateTime { get; set; }
        public DateTime EndedDateTime { get; set; }
        public int HostId { get; set; }

        public bool ActiveData { get; set; } = true;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }

        public IEnumerable<QuizParticipant> GetParticipants() {
            return JsonConvert.DeserializeObject<IEnumerable<QuizParticipant>>(ParticipantsJSON) ?? new List<QuizParticipant>();
        }

        public IEnumerable<SetQuizRoom> GetSetQuizRooms()
        {
            return JsonConvert.DeserializeObject<IEnumerable<SetQuizRoom>>(SetQuizRoomJSON) ?? new List<SetQuizRoom>();
        }
    }
}
