namespace QuizMaster.API.Quiz.Models.Questions
{
	public class SliderQuestion : IQuestion<IEnumerable<int>>
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
		public IEnumerable<int> Answer { get ; set ; }

        public SliderQuestion(string statement): this(statement, new List<int>())
        {
            
        }

        public SliderQuestion(string statement, IEnumerable<int> answer)
        {
            Statement = statement;
			Answer = answer;
        }
    }
}
