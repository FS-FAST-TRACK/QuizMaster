namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDetailsCreateDto
	{
		public IEnumerable<QuestionDetailCreateDto> QuestionDetailCreateDtos { get; set; }
	}
	public class QuestionDetailCreateDto
	{
		public string DetailDesc { get; set; }
		public IEnumerable<int> DetailTypeIds { get; set; }
	}
}
