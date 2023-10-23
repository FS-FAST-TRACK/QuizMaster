using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Quiz.DbContexts;
using QuizMaster.Library.Common.Entities.Questionnaire;

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

		#region Category Methods
		public async Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync()
		{	
			return await _context.Categories.Where(c => c.ActiveData).ToListAsync();
		}

		public async Task<QuestionCategory?> GetCategoryAsync(int id)
		{
			return await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
		}

		public async Task<QuestionCategory?> GetCategoryAsync(string description)
		{
			return await _context.Categories.Where(c => c.QCategoryDesc.Trim().ToLower().Replace(" ","") == description.Trim().ToLower().Replace(" ", "")).FirstOrDefaultAsync();
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
		
		public  bool UpdateCategory(QuestionCategory category)
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

		public async Task<QuestionDifficulty?> GetDifficultyAsync(int id)
		{
			return await _context.Difficulties.Where(d =>d.Id == id).FirstOrDefaultAsync();
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


		public async Task<bool> SaveChangesAsync()
		{
			var result = await _context.SaveChangesAsync();
			return result != 0;
		}
	}
}
