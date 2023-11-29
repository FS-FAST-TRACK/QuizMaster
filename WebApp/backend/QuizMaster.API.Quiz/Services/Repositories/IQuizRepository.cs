using QuizMaster.API.Quiz.Models;
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
		Task<PagedList<CategoryDto>> GetAllCategoriesAsync(CategoryResourceParameter resourceParameter);
        Task<QuestionCategory?> GetCategoryAsync(int id);
		Task<QuestionCategory?> GetCategoryAsync(string description);
		Task<bool> AddCategoryAsync(QuestionCategory category);
		bool UpdateCategory(QuestionCategory category);


		Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync();
        Task<PagedList<DifficultyDto>> GetAllDifficultiesAsync(DifficultyResourceParameter resourceParameter);
        Task<QuestionDifficulty?> GetDifficultyAsync(int id);
		Task<QuestionDifficulty?> GetDifficultyAsync(string description);
		Task<bool> AddDifficultyAsync(QuestionDifficulty difficulty);
		bool UpdateDifficulty(QuestionDifficulty difficulty);


		Task<IEnumerable<QuestionType>> GetAllTypesAsync();
		Task<QuestionType?> GetTypeAsync(int id);
		Task<QuestionType?> GetTypeAsync(string description);
		Task<bool> AddTypeAsync(QuestionType type);
		bool UpdateType(QuestionType type);

		Task<IEnumerable<QuestionDetail>> GetQuestionDetailsAsync(int qId);
        Task<IEnumerable<QuestionDetail>> GetAllQuestionDetailsAsync();
        Task<IEnumerable<QuestionDetail>> GetQuestionDetailByDetailTypeAsync(int qId, int detailTypeId);
		Task<QuestionDetail?> GetQuestionDetailAsync(int qId, int id);
		Task<bool> AddQuestionDetailAsync(QuestionDetail questionDetail);
		Task<bool> AddQuestionDetailsAsync(IEnumerable<QuestionDetail> questionDetails);
		bool UpdateQuestionDetail(QuestionDetail questionDetail);		

		Task<IEnumerable<DetailType>> GetDetailTypesAsync();
		Task<DetailType?> GetDetailTypeAsync(int id);
		Task<Dictionary<string, DetailType>> GetDetailTypesDictAsync();
		Task<IEnumerable<DetailType>> GetDetailTypesAsync(IEnumerable<string> detailTypes);

		Task<bool> AddQuestionDetailTypesAsync(IEnumerable<QuestionDetailType> questionDetailTypes);
		Task<bool> AddQuestionDetailTypeAsync(QuestionDetailType questionDetailType);
		Task<bool> RemoveQuestionDetailTypesOfQuestionDetailByIdAsync(int qDetailId);
		Task<IEnumerable<QuestionDetailType>> GetAllQuestionDetailTypesAsync();

		Task<int> GetQuestionUseCategoryCount(int categoryId);
		Task<int> GetQuestionUseDifficultyCount(int difficultyId);
		Task<int> GetQuestionUseTypeCount(int typeId);
		Task<bool> SaveChangesAsync();

	}
}
