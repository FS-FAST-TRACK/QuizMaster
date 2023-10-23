using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services.Repositories
{
	public interface IQuizRepository
	{
		Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync();
		Task<QuestionCategory?> GetCategoryAsync(int id);
		Task<QuestionCategory?> GetCategoryAsync(string description);
		Task<bool> AddCategoryAsync(QuestionCategory category);
		bool UpdateCategory(QuestionCategory category);


		Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync();
		Task<QuestionDifficulty?> GetDifficultyAsync(int id);
		Task<QuestionDifficulty?> GetDifficultyAsync(string description);
		Task<bool> AddDifficultyAsync(QuestionDifficulty difficulty);
		bool UpdateDifficulty(QuestionDifficulty difficulty);


		Task<IEnumerable<QuestionType>> GetAllTypesAsync();
		Task<QuestionType?> GetTypeAsync(int id);
		Task<QuestionType?> GetTypeAsync(string description);
		Task<bool> AddTypeAsync(QuestionType type);
		bool UpdateType(QuestionType type);
		Task<bool> SaveChangesAsync();

	}
}
