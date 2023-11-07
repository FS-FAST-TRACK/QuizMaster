namespace QuizMaster.API.Quiz.Models.Questions
{
	public class PuzzleQuestion : IQuestion<IEnumerable<Option>>
	{
		public int Id { get; set; }
		public string Statement { get; set; }
		public int Time { get; set; }
		public string? Audio { get; set; }
		public string? Video { get; set; }
		public string? Image { get; set; }
		public int DifficultyId { get; set; }
		public int CategoryId { get; set; }
		public int TypeId { get; set; }
		public IEnumerable<Option> Answer { get; set; }

		public PuzzleQuestion(string statement) : this(statement, new List<Option>())
		{

		}
		public PuzzleQuestion(string statement, IEnumerable<Option> answer)
		{
			Statement = statement;
			Answer = answer;
		}
	}
}
