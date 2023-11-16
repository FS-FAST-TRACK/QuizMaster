using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services
{
	public interface IQuestionDetailManager
	{	
		Task<bool> AddQuestionDetailAsync(Question question, QuestionDetail questionDetail);
		Task<bool> AddQuestionDetailAsync(Question question, QuestionDetailCreateDto questionDetailCreateDto);
		Task<bool> AddQuestionDetailAsync(Question question, IEnumerable<QuestionDetailCreateDto> questionDetailCreateDto);
		Task<bool> UpdateQuestionDetailAsync(Question question, QuestionDetail questionDetail);
	}
}
