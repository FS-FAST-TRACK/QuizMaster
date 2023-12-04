using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Rooms
{
    [PrimaryKey(nameof(QSetId), nameof(QRoomId))]
    public class SetQuizRoom : IEntity
    {
        [ForeignKey(nameof(Set))]
        public int QSetId { get; set; }
        public Set QSet { get; set; }
        [ForeignKey(nameof(QuizRoom))]
        public int QRoomId { get; set; }
        public QuizRoom QRoom { get; set; }
        public bool ActiveData { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }
    }
}
