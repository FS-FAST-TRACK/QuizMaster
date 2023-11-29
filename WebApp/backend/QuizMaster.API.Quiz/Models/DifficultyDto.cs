using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.API.Quiz.Models
{
	public class DifficultyDto
	{
		public int Id { get; set; }

		public string QDifficultyDesc { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

		public int QuestionCounts { get; set; }
    }
}
