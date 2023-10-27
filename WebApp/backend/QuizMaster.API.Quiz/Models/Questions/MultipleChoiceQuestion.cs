using System.Collections.Generic;

namespace QuizMaster.API.Quiz.Models.Questions
{
	public class MultipleChoiceQuestion : IQuestion<IEnumerable<Option>>
	{
		public int Id { get; set; }
		public string Statement { get; set; }
		public string? Audio { get; set; }
		public string? Video { get; set; }
		public string? Image { get; set; }
		public int DifficultyId { get; set; }
		public int CategoryId { get; set; }
		public int TypeId { get; set; }
		public int Time { get; set; }
		public IEnumerable<Option> Choices {get; set;}
		public IEnumerable<Option> Answer { get; set; }

		public MultipleChoiceQuestion(string statement): this(statement,new List<Option>(), new List<Option>())
		{
        }
        public MultipleChoiceQuestion(string statement, IEnumerable<Option> options, IEnumerable<Option> answer)
        {
            Statement = statement ;
			Choices = options;
			Answer = answer;
        }
    }
}
