namespace QuizMaster.API.Quiz.Models.Questions
{
	public class TrueOrFalseQuestion : IQuestion<bool>
	{
		public int Id { get ; set ; }
		public string Statement { get ; set ; }
		public int Time { get ; set ; }
		public string? Audio { get ; set ; }
		public string? Video { get ; set ; }
		public string? Image { get ; set ; }
		public int DifficultyId { get ; set ; }
		public int CategoryId { get ; set ; }
		public int TypeId { get ; set ; }

		public bool Answer { get; set;  }

        public TrueOrFalseQuestion(string statement)
        {
            Statement = statement ;
        }
    }
}
