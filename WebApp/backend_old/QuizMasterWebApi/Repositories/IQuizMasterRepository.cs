using QuizMaster.Common.Library.Entities;

namespace QuizMaster.Account.Api.Repositories
{
	public interface IQuizMasterRepository
	{
		Task AddUserAsync(UserAccount userAccount);
		void UpdateUser(UserAccount userAccount);
		Task<UserAccount?> GetUserAsync(string userName);
		Task<bool> IsUserExistsByUserNameAsync(string userName);
		Task<bool> IsUserExistsAsync(UserAccount user);
		Task<UserAccount?> GetUserAsync(int id);
		Task<IEnumerable<UserAccount>> GetUsersAsync();
		Task<bool> SaveChangesAsync();
	}
}
