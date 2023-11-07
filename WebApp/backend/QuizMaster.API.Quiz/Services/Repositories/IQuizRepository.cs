using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers.Quiz;

namespace QuizMaster.API.Quiz.Services.Repositories
{
	public interface IQuizRepository
	{
		Task<IEnumerable<Question>> GetAllQuestionsAsync();
		Task<PagedList<Question>> GetAllQuestionsAsync(QuestionResourceParameter resourceParameter);
		Task<Question?> GetQuestionAsync(int id);
		Task<Question?> GetQuestionAsync(string qStatement, int difficultyId, int typeId, int categoryId);
		Task<bool> AddQuestionAsync(Question question);
		bool UpdateQuestion(Question question);


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

		Task<QuestionDetail?> GetQuestionDetailAsync(int qId);
		Task<bool> AddQuestionDetailsAsync(QuestionDetail detail);
		bool UpdateQuestionDetail(QuestionDetail detail);

		Task<int> GetQuestionUseCategoryCount(int categoryId);
		Task<int> GetQuestionUseDifficultyCount(int difficultyId);
		Task<int> GetQuestionUseTypeCount(int typeId);
		Task<bool> SaveChangesAsync();

	}
}
