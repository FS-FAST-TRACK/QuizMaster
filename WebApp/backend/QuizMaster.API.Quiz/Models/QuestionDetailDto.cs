namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDetailDto
	{
		public int Id { get; set; }
		public string QDetailDesc { get; set; }
		public IEnumerable<DetailTypeDto> DetailTypes { get; set; }
	}
}
