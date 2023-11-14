namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDetailsCreateDto
	{
		public IEnumerable<QuestionDetailCreateDto> QuestionDetailCreateDtos { get; set; }
	}
	public class QuestionDetailCreateDto
	{
		public string QDetailDesc { get; set; }
		public IEnumerable<string> DetailTypes { get; set; }
	}
}
