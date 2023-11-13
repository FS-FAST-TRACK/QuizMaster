namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDetailDto
	{
		public int Id { get; set; }
		public DetailDto Detail { get; set; }
		public DetailTypeDto QuestionDetailType { get; set; }
	}
}
