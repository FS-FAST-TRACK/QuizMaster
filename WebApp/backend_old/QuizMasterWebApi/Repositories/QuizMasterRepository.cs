using Microsoft.EntityFrameworkCore;
using QuizMaster.Account.Api.DbContexts;
using QuizMaster.Common.Library.Entities;

namespace QuizMaster.Account.Api.Repositories
{
	public class QuizMasterRepository : IQuizMasterRepository
	{
		private readonly QuizMasterDbContext _context;
		public QuizMasterRepository(QuizMasterDbContext quizMasterDbContext)
		{
			_context = quizMasterDbContext;
		}

		public async Task AddUserAsync(UserAccount userAccount)
		{
			await _context.UserAccounts.AddAsync(userAccount);
		}

		public async Task<UserAccount?> GetUserAsync(string userName)
		{
			var user = await _context.UserAccounts.Include(user => user.UserRole).Where(user => user.UserName == userName && user.ActiveData ).FirstOrDefaultAsync();
			return user;
		}

		public async Task<UserAccount?> GetUserAsync(int id)
		{
			return await _context.UserAccounts.Include(user => user.UserRole).Where(user => user.Id == id && user.ActiveData == true).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<UserAccount>> GetUsersAsync()
		{
			return await _context.UserAccounts.ToListAsync();
		}

		public async Task<bool> IsUserExistsByUserNameAsync(string userName)
		{
			return await _context.UserAccounts.Where(user => user.UserName == userName).AnyAsync();
		}

		public async Task<bool> IsUserExistsAsync(UserAccount user)
		{
			return await _context.UserAccounts.Where(u => u.Id != user.Id && u.UserName == user.UserName).AnyAsync();
		}


		public void UpdateUser(UserAccount userAccount)
		{
			_context.UserAccounts.Update(userAccount);
		}

		public async Task<bool> SaveChangesAsync()
		{
			return ( await _context.SaveChangesAsync() >= 0 );
		}

	}
}
