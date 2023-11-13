using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services
{
	public interface IQuestionDetailManager
	{	
		Task<bool> AddQuestionDetail(Question question, QuestionDetail questionDetail);
		Task<bool> AddQuestionDetail(Question question, QuestionDetailCreateDto questionDetailCreateDto);
		Task<bool> AddQuestionDetail(Question question, IEnumerable<QuestionDetailCreateDto> questionDetailCreateDto);
	}
}
