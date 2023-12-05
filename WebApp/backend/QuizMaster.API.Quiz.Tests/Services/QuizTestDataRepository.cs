using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers.Quiz;

namespace QuizMaster.API.Quiz.Tests.Services
{

	public class QuizTestDataRepository : IQuizRepository
	{
		private readonly IEnumerable<QuestionCategory> _categories;
		private readonly IEnumerable<QuestionType> _types;
		private readonly IEnumerable<QuestionDifficulty> _difficulties;
		private readonly IEnumerable<Question> _questions;
		private readonly IEnumerable<QuestionDetail> _questionsDetails;

		public QuizTestDataRepository()
		{
			_categories = QuestionCategories.Categories;
			_types = QuestionTypes.Types;
			_difficulties = QuestionDifficulties.Difficulties;
			_questions = new List<Question>();
			_questionsDetails = new List<QuestionDetail>();
		}

		public async Task<bool> AddCategoryAsync(QuestionCategory category)
		{
			try
			{
				await Task.FromResult(() => _categories.Append(category));
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<bool> AddDifficultyAsync(QuestionDifficulty difficulty)
		{
			try
			{
				await Task.FromResult(() => _difficulties.Append(difficulty));
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public Task<bool> AddQuestionAsync(Question question)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddQuestionDetailAsync(QuestionDetail questionDetail)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddQuestionDetailsAsync(QuestionDetail detail)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddQuestionDetailsAsync(IEnumerable<QuestionDetail> questionDetails)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddQuestionDetailTypeAsync(QuestionDetailType questionDetailType)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddQuestionDetailTypesAsync(IEnumerable<QuestionDetailType> questionDetailTypes)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> AddTypeAsync(QuestionType type)
		{
			try
			{
				await Task.FromResult(() => _types.Append(type));
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync()
		{
			return await Task.FromResult(_categories);
		}

		public Task<PagedList<QuestionCategory>> GetAllCategoriesAsync(CategoryResourceParameter resourceParameter)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync()
		{
			return await Task.FromResult(_difficulties);
		}

        public Task<IEnumerable<QuestionDetail>> GetAllQuestionDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionDetailType>> GetAllQuestionDetailTypesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetAllQuestionsAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<PagedList<Question>> GetAllQuestionsAsync(QuestionResourceParameter resourceParameter)
		{
			var collection = _questions.AsQueryable();
			return await Task.FromResult(PagedList<Question>.Create(collection, resourceParameter.PageNumber, resourceParameter.PageSize));
		}

		public Task<IEnumerable<QuestionType>> GetAllTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<QuestionCategory?> GetCategoryAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionCategory?> GetCategoryAsync(string description)
		{
			throw new NotImplementedException();
		}
		public Task<DetailType?> GetDetailTypeAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<DetailType>> GetDetailTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<DetailType>> GetDetailTypesAsync(IEnumerable<string> detailTypes)
		{
			throw new NotImplementedException();
		}

		public Task<Dictionary<string, DetailType>> GetDetailTypesDictAsync()
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDifficulty?> GetDifficultyAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDifficulty?> GetDifficultyAsync(string description)
		{
			throw new NotImplementedException();
		}

		public Task<Question?> GetQuestionAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Question?> GetQuestionAsync(string qStatement, int difficultyId, int typeId, int categoryId)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDetail?> GetQuestionDetailAsync(int qId)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDetail?> GetQuestionDetailAsync(int qId, int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<QuestionDetail>> GetQuestionDetailByDetailTypeAsync(int qId, int detailTypeId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<QuestionDetail>> GetQuestionDetailsAsync(int qId)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionDetailType?> GetQuestionDetailTypeAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<QuestionDetailType>> GetQuestionDetailTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<int> GetQuestionUseCategoryCount(int categoryId)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetQuestionUseDifficultyCount(int difficultyId)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetQuestionUseTypeCount(int typeId)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionType?> GetTypeAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<QuestionType?> GetTypeAsync(string description)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveQuestionDetailTypesOfQuestionDetailByIdAsync(int qDetailId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> SaveChangesAsync()
		{
			throw new NotImplementedException();
		}

		public bool UpdateCategory(QuestionCategory category)
		{
			throw new NotImplementedException();
		}

		public bool UpdateDifficulty(QuestionDifficulty difficulty)
		{
			throw new NotImplementedException();
		}

		public bool UpdateQuestion(Question question)
		{
			throw new NotImplementedException();
		}

		public bool UpdateQuestionDetail(QuestionDetail detail)
		{
			throw new NotImplementedException();
		}

		public bool UpdateType(QuestionType type)
		{
			throw new NotImplementedException();
		}
	}
}
