using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Quiz.DbContexts;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers.Quiz;

namespace QuizMaster.API.Quiz.Services.Repositories
{
	public class QuizRepository : IQuizRepository
	{
		#region Public Constructor and private fields
		private readonly QuestionDbContext _context;
		private readonly ILogger<QuizRepository> _logger;

		public QuizRepository(QuestionDbContext context, ILogger<QuizRepository> logger)
		{
			_context = context;
			_logger = logger;
		}
		#endregion


		#region Question Methods
		public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
		{
			return await _context.Questions
				.Where(q => q.ActiveData)
				.Include(q => q.QCategory)
				.Include(q => q.QDifficulty)
				.Include(q => q.QType)
				.Include(q => q.Details)
				.ToListAsync();

		}
		public async Task<PagedList<Question>> GetAllQuestionsAsync(QuestionResourceParameter resourceParameter)
		{
			var collection = _context.Questions as IQueryable<Question>;

			if (resourceParameter.IsOnlyActiveData)
			{
				collection = collection.Where(q => q.ActiveData);
			}

			if (resourceParameter.QCategoryId != null)
			{
				collection = collection.Where(q => q.QCategoryId == resourceParameter.QCategoryId);
			}

			if(resourceParameter.QDifficultyId != null)
			{
				collection = collection.Where(q => q.QDifficultyId == resourceParameter.QDifficultyId);
			}

			if (resourceParameter.QTypeId != null)
			{
				collection = collection.Where(q => q.QTypeId == resourceParameter.QTypeId);
			}
			collection = collection
				.Include(q => q.QCategory)
				.Include(q => q.QDifficulty)
				.Include(q => q.QType);

			
			if (!string.IsNullOrWhiteSpace(resourceParameter.SearchQuery))
			{
				var query = resourceParameter.SearchQuery.ToLower().Replace(" ", "");
				collection = collection
					.Where(q =>
					q.QStatement.ToLower().Replace(" ", "").Contains(query)
					|| q.QCategory.QCategoryDesc.ToLower().Replace(" ", "").Contains(query)
					|| q.QType.QTypeDesc.ToLower().Replace(" ", "").Contains(query)
					|| q.QDifficulty.QDifficultyDesc.ToLower().Replace(" ", "").Contains(query)
					);

			}

			return await PagedList<Question>.CreateAsync(collection,
				resourceParameter.PageNumber,
				resourceParameter.PageSize);
		}

		public async Task<Question?> GetQuestionAsync(int id)
		{
			var question = await _context.Questions
				.Where(q => q.Id == id)
				.Include(q => q.QCategory)
				.Include(q => q.QDifficulty)
				.Include(q => q.QType)
				.Include(q => q.Details)
				.FirstOrDefaultAsync();

			if (question != null)
			{
				question.Details.ToList().ForEach(qDetail =>
				{
					qDetail.DetailTypes = _context.QuestionDetailTypes.Where(qDetailType => qDetailType.QuestionDetailId == qDetail.Id).Select((qDetailType) =>
					 qDetailType.DetailType).ToList();
				});
			}

			return question;


		}

		public async Task<Question?> GetQuestionAsync(string qStatement, int difficultyId, int typeId, int categoryId)
		{
			return await _context.Questions.Where(q =>
				q.QDifficulty.Id == difficultyId
				&& q.QType.Id == typeId
				&& q.QCategory.Id == categoryId
				&& q.QStatement.Trim().ToLower().Replace(" ", "") == qStatement.Trim().ToLower().Replace(" ", ""))
				.FirstOrDefaultAsync();
		}

		public async Task<bool> AddQuestionAsync(Question question)
		{
			try
			{
				await _context.Questions.AddAsync(question);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add Question Failed", ex);
				return false;
			}
		}

		public bool UpdateQuestion(Question question)
		{
			try
			{
				_context.Questions.Update(question);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Update Question Failed", ex);
				return false;
			}
		}
		#endregion

		#region Category Methods
		public async Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync()
		{
			return await _context.Categories.Where(c => c.ActiveData).ToListAsync();
		}

		public async Task<PagedList<CategoryDto>> GetAllCategoriesAsync(CategoryResourceParameter resourceParameter)
		{
			var collection = _context.Categories as IQueryable<QuestionCategory>;
			if (resourceParameter.IsOnlyActiveData)
			{
				collection = collection.Where(c => c.ActiveData);
			}
			

			if (!string.IsNullOrWhiteSpace(resourceParameter.SearchQuery))
			{
				var query = resourceParameter.SearchQuery.ToLower().Replace(" ", "");
				collection = collection
					.Where(c =>
					c.QCategoryDesc.ToLower().Replace(" ", "").Contains(query)
					
					);

			}
			var collection2 = collection.Select(c => new CategoryDto
			{
				Id = c.Id,
				QCategoryDesc = c.QCategoryDesc,
				DateCreated = c.DateCreated,
				DateUpdated = c.DateUpdated,
				QuestionCounts = _context.Questions.Where(q => q.ActiveData && q.QCategoryId == c.Id).Count()
			})as IQueryable<CategoryDto>;


			return await PagedList<CategoryDto>.CreateAsync(collection2,
				resourceParameter.PageNumber,
				resourceParameter.PageSize);

		}

		public async Task<QuestionCategory?> GetCategoryAsync(int id)
		{
			return await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
		}

		public async Task<QuestionCategory?> GetCategoryAsync(string description)
		{
			return await _context.Categories.Where(c => c.QCategoryDesc.Trim().ToLower().Replace(" ", "") == description.Trim().ToLower().Replace(" ", "")).FirstOrDefaultAsync();
		}


		public async Task<bool> AddCategoryAsync(QuestionCategory category)
		{
			try
			{
				await _context.Categories.AddAsync(category);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add Category Failed", ex);
				return false;
			}
		}

		public bool UpdateCategory(QuestionCategory category)
		{
			try
			{
				_context.Categories.Update(category);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Update Category Failed", ex);
				return false;
			}
		}

		#endregion

		#region Difficulty Methods
		public async Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync()
		{
			return await _context.Difficulties.Where(d => d.ActiveData).ToListAsync();
		}

        public async Task<PagedList<DifficultyDto>> GetAllDifficultiesAsync(DifficultyResourceParameter resourceParameter)
		{
            var collection = _context.Difficulties as IQueryable<QuestionDifficulty>;
            if (resourceParameter.IsOnlyActiveData)
            {
                collection = collection.Where(c => c.ActiveData);
            }


            if (!string.IsNullOrWhiteSpace(resourceParameter.SearchQuery))
            {
                var query = resourceParameter.SearchQuery.ToLower().Replace(" ", "");
                collection = collection
                    .Where(c =>
                    c.QDifficultyDesc.ToLower().Replace(" ", "").Contains(query)

                    );

            }
            var collection2 = collection.Select(c => new DifficultyDto
            {
                Id = c.Id,
                QDifficultyDesc = c.QDifficultyDesc,
                DateCreated = c.DateCreated,
                DateUpdated = c.DateUpdated,
                QuestionCounts = _context.Questions.Where(q => q.ActiveData && q.QCategoryId == c.Id).Count()
            }) as IQueryable<DifficultyDto>;


            return await PagedList<DifficultyDto>.CreateAsync(collection2,
                resourceParameter.PageNumber,
                resourceParameter.PageSize);

        }

        public async Task<QuestionDifficulty?> GetDifficultyAsync(int id)
		{
			return await _context.Difficulties.Where(d => d.Id == id).FirstOrDefaultAsync();
		}

		public async Task<QuestionDifficulty?> GetDifficultyAsync(string description)
		{
			return await _context.Difficulties.Where(d => d.QDifficultyDesc.Trim().ToLower().Replace(" ", "") == description.Trim().ToLower().Replace(" ", "")).FirstOrDefaultAsync();
		}


		public async Task<bool> AddDifficultyAsync(QuestionDifficulty difficulty)
		{
			try
			{
				await _context.Difficulties.AddAsync(difficulty);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add Difficulty Failed", ex);
				return false;
			}
		}

		public bool UpdateDifficulty(QuestionDifficulty difficulty)
		{
			try
			{
				_context.Difficulties.Update(difficulty);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Update Difficulty Failed", ex);
				return false;
			}
		}
		#endregion

		#region Type Methods
		public async Task<IEnumerable<QuestionType>> GetAllTypesAsync()
		{
			return await _context.Types.Where(c => c.ActiveData).ToListAsync();
		}

		public async Task<QuestionType?> GetTypeAsync(int id)
		{
			return await _context.Types.Where(c => c.Id == id).FirstOrDefaultAsync();
		}

		public async Task<QuestionType?> GetTypeAsync(string description)
		{
			return await _context.Types.Where(c => c.QTypeDesc.Trim().ToLower().Replace(" ", "") == description.Trim().ToLower().Replace(" ", "")).FirstOrDefaultAsync();
		}


		public async Task<bool> AddTypeAsync(QuestionType type)
		{
			try
			{
				await _context.Types.AddAsync(type);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add Type Failed", ex);
				return false;
			}
		}

		public bool UpdateType(QuestionType type)
		{
			try
			{
				_context.Types.Update(type);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Update Type Failed", ex);
				return false;
			}
		}
		#endregion

		#region Question Detail Methods
		public async Task<IEnumerable<QuestionDetail>> GetQuestionDetailsAsync(int qId)
		{
			var details = await _context.QuestionDetails
				.Where(qDetail => qDetail.Question.Id == qId && qDetail.ActiveData)
				.Include(qDetail => qDetail.DetailTypes)
				.ToListAsync();
			details.ToList().ForEach(qDetail =>
			{
				qDetail.DetailTypes = _context.QuestionDetailTypes.Where(qDetailType => qDetailType.QuestionDetailId == qDetail.Id).Select((qDetailType) =>
				 qDetailType.DetailType).ToList();
			});
			return details;
		}

		public async Task<QuestionDetail?> GetQuestionDetailAsync(int qId, int id)
		{
			var qDetail = await _context.QuestionDetails
				.Where(qDetail => qDetail.Question.Id == qId && qDetail.Id == id)
				.Include(qDetail => qDetail.DetailTypes)
				.SingleOrDefaultAsync();

			if (qDetail != null)
			{
				qDetail.DetailTypes = _context.QuestionDetailTypes.Where(qDetailType => qDetailType.QuestionDetailId == qDetail.Id).Select((qDetailType) => qDetailType.DetailType).ToList();
			}
			return qDetail;
		}

        public async Task<IEnumerable<QuestionDetail>> GetAllQuestionDetailsAsync()
        {
			return await _context.QuestionDetails.ToListAsync();
        }

        public async Task<IEnumerable<QuestionDetail>> GetQuestionDetailByDetailTypeAsync(int qId, int detailTypeId)
		{
			return await _context.QuestionDetails
				.Where(qDetail => qDetail.QuestionId == qId)
				.Include(qDetail => qDetail.DetailTypes.Where(dType => dType.Id == detailTypeId))
				.ToListAsync();
		}

		public async Task<bool> AddQuestionDetailAsync(QuestionDetail questionDetail)
		{
			try
			{
				
				await _context.QuestionDetails.AddAsync(questionDetail);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Adding Detail Failed", ex);
				return false;
			}
		}

		public async Task<bool> AddQuestionDetailsAsync(IEnumerable<QuestionDetail> questionDetails)
		{
			try
			{
				await _context.QuestionDetails.AddRangeAsync(questionDetails);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Adding Detail Failed", ex);
				return false;
			}
		}


		public bool UpdateQuestionDetail(QuestionDetail questionDetail)
		{
			try
			{
				_context.QuestionDetails.Update(questionDetail);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Adding Detail Failed", ex);
				return false;
			}
		}
		#endregion

		#region Detail Type Methods
		public async Task<IEnumerable<DetailType>> GetDetailTypesAsync()
		{
			return await _context.DetailTypes.Where(dType => dType.ActiveData).ToListAsync();
		}

		public async Task<DetailType?> GetDetailTypeAsync(int id)
		{
			return await _context.DetailTypes.Where(dType => dType.Id == id && dType.ActiveData).FirstOrDefaultAsync();
		}

		public async Task<Dictionary<string, DetailType>> GetDetailTypesDictAsync()
		{
			var detailTypesDict = new Dictionary<string, DetailType>();
			var detailTypes = await _context.DetailTypes.ToListAsync();

			DetailTypes.keyValuePairs.ToList().ForEach(kv =>
			{
				var detailType = detailTypes.Where(dType => dType.Id == kv.Value).FirstOrDefault();
				if (detailType != null)
				{
					detailTypesDict.Add(kv.Key, detailType);
				}
			});
			return detailTypesDict;
		}


		public async Task<IEnumerable<DetailType>> GetDetailTypesAsync(IEnumerable<string> detailTypes)
		{
			var detailTypeIds = new List<int>();
			detailTypes.ToList().ForEach(dT =>
				detailTypeIds.Add(DetailTypes.keyValuePairs[dT]
			));


			var detailTypesDb = _context.DetailTypes;
			var query = detailTypesDb.Where(detailTypeDb => detailTypeIds.Contains(detailTypeDb.Id));
			return await query.ToListAsync();

		}

		#endregion

		#region Question - Detail Type Methods
		public async Task<bool> AddQuestionDetailTypesAsync(IEnumerable<QuestionDetailType> questionDetailTypes)
		{
			try
			{
				await _context.QuestionDetailTypes.AddRangeAsync(questionDetailTypes);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add QuestionDetailTypes Failed", ex);
				return false;
			}
		}

		public async Task<bool> AddQuestionDetailTypeAsync(QuestionDetailType questionDetailType)
		{
			try
			{
				await _context.QuestionDetailTypes.AddAsync(questionDetailType);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add QuestionDetailTypes Failed", ex);
				return false;
			}
		}

		public async Task<bool> RemoveQuestionDetailTypesOfQuestionDetailByIdAsync(int qDetailId){
			try
			{
				var questionDetailTypes = await _context.QuestionDetailTypes.Where(qDetailType => qDetailType.QuestionDetailId == qDetailId).ToListAsync();
				_context.QuestionDetailTypes.RemoveRange(questionDetailTypes);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Add QuestionDetailTypes Failed", ex);
				return false;
			}
		}
		public async Task<IEnumerable<QuestionDetailType>> GetAllQuestionDetailTypesAsync()
		{
			return await _context.QuestionDetailTypes.ToListAsync();
		}

        #endregion


        // Returns how many active questions used a question category
        public async Task<int> GetQuestionUseCategoryCount(int categoryId)
		{
			return await _context.Questions.Where(q => q.ActiveData && q.QCategory.Id == categoryId).CountAsync();
		}

		// Returns how many questions used a question difficulty
		public async Task<int> GetQuestionUseDifficultyCount(int difficultyId)
		{
			return await _context.Questions.Where(q => q.ActiveData && q.QDifficulty.Id == difficultyId).CountAsync();
		}

		// Returns how many active questions used a question type
		public async Task<int> GetQuestionUseTypeCount(int typeId)
		{
			return await _context.Questions.Where(q => q.ActiveData && q.QType.Id == typeId).CountAsync();
		}

		public async Task<bool> SaveChangesAsync()
		{
			var result = await _context.SaveChangesAsync();
			return result != 0;
		}

        
    }
}
