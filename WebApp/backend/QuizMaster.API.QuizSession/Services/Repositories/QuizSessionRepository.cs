using Microsoft.EntityFrameworkCore;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.QuizSession.Services.Repositories
{
    public class QuizSessionRepository : IQuizSessionRepository
    {
        private readonly QuizSessionDbContext _context;

        public QuizSessionRepository(QuizSessionDbContext context)
        {
            _context = context;
        }

        public QuizSessionDbContext QuizSessionDbContext => _context;

        public async Task<IEnumerable<QuestionCategory>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Where(c => c.ActiveData).ToListAsync();
        }

        public async Task<IEnumerable<QuestionDifficulty>> GetAllDifficultiesAsync()
        {
            return await _context.Difficulties.Where(d => d.ActiveData).ToListAsync();
        }

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

        public async Task<IEnumerable<QuestionType>> GetAllTypesAsync()
        {
            return await _context.Types.Where(t => t.ActiveData).ToListAsync();
        }

        public async Task<QuestionCategory?> GetCategoryAsync(int id)
        {
            return await _context.Categories.Where(c => c.ActiveData && c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<QuestionDifficulty?> GetDifficultyAsync(int id)
        {
            return await _context.Difficulties.Where(d => d.ActiveData && d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Question?> GetQuestionAsync(int id)
        {
            return await _context.Questions
                .Where(q => q.Id == id)
                .Include(q => q.QCategory)
                .Include(q => q.QDifficulty)
                .Include(q => q.QType)
                .Include(q => q.Details)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QuestionDetail>> GetQuestionDetailByDetailTypeAsync(int qId, int detailTypeId)
        {
            return await _context.QuestionDetails.Where(q => q.QuestionId == qId).Include(qDetail => qDetail.DetailTypes.Where(t => t.Id == detailTypeId)).ToListAsync();
        }

        public async Task<IEnumerable<QuestionDetail>> GetQuestionDetailsAsync(int qId)
        {
            return await _context.QuestionDetails
                .Where(qDetail => qDetail.Question.Id == qId)
                .Include(qDetail => qDetail.DetailTypes)
                .ToListAsync();
        }

        public async Task<QuestionType?> GetTypeAsync(int id)
        {
            return await _context.Types.Where(c => c.Id == id).FirstOrDefaultAsync();
        }
    }
}
