using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers.Quiz;

namespace QuizMaster.API.QuizSession.Services.Repositories
{
	public interface IQuizSessionRepository
	{
		Task<IEnumerable<Question>> GetAllQuestionsAsync();
		Task<Question?> GetQuestionAsync(int id);

		Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync();
		Task<QuestionCategory?> GetCategoryAsync(int id);


		Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync();
		Task<QuestionDifficulty?> GetDifficultyAsync(int id);


		Task<IEnumerable<QuestionType>> GetAllTypesAsync();
		Task<QuestionType?> GetTypeAsync(int id);

		Task<IEnumerable<QuestionDetail>> GetQuestionDetailsAsync(int qId);
		Task<IEnumerable<QuestionDetail>> GetQuestionDetailByDetailTypeAsync(int qId, int detailTypeId);
		QuizSessionDbContext QuizSessionDbContext { get; }
	}
}
