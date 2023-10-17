using Microsoft.EntityFrameworkCore;
using QuizMaster.Common.Library.Entities;
using QuizMaster.Common.Library.Roles;
using QuizMaster.Common.Library.Utility;

namespace QuizMaster.Account.Api.DbContexts
{
	public class QuizMasterDbContext : DbContext
	{
		public DbSet<UserRole> UserRoles { get; set; }
		public DbSet<QuestionType> QuestionTypes { get; set; }
		public DbSet<QuestionDetail> QuestionDetails { get; set; }
		public DbSet<QuestionDifficulty> QuestionDifficulties { get; set; }
		public DbSet<QuestionCategory> QuestionCategories { get; set; }
		public DbSet<Question> Questions { get; set; }

		public DbSet<UserAccount> UserAccounts { get; set; }

		public QuizMasterDbContext(DbContextOptions<QuizMasterDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var user = new UserAccount()
			{
				Id = 1,
				LastName = "admin",
				FirstName = "admin",
				EmailAddress = "admin@fullscale.io",
				UserName = "admin",
				UserRoleId = RolesData.Admin.Id,
				Password = Hash.GetHashSha256("P@ssw0rd"),
				DateCreated = DateTime.Now,

			};

			var admin = new UserAccount
			{
				Id = 2,
				LastName = "user",
				FirstName = "user",
				EmailAddress = "user@fullscale.io",
				UserName = "user",
				UserRoleId = RolesData.User.Id,
				Password = Hash.GetHashSha256("P@ssw0rd"),
				DateCreated = DateTime.Now,
			};

			modelBuilder.Entity<UserRole>()
				.HasData(RolesData.User, RolesData.Admin);

			modelBuilder.Entity<UserAccount>()
				.HasData(user, admin);

			modelBuilder.Entity<QuestionDifficulty>()
					.HasData(new QuestionDifficulty()
					{
						Id = 1,
						QDifficultyDesc = "Easy",
						DateCreated = DateTime.Now,
						UserId = admin.Id,
					}, new QuestionDifficulty()
					{
						Id = 2,
						QDifficultyDesc = "Average",
						DateCreated = DateTime.Now,
						UserId = admin.Id,
					}, new QuestionDifficulty()
					{
						Id = 3,
						QDifficultyDesc = "Difficult",
						DateCreated = DateTime.Now,
						UserId = admin.Id,
					}
					);


			modelBuilder.Entity<QuestionCategory>()
				.HasData(
				new QuestionCategory()
				{
					Id = 1,
					QCategoryDesc = "Science",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 2,
					QCategoryDesc = "Movies",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 3,
					QCategoryDesc = "Animals",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 4,
					QCategoryDesc = "Places",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 5,
					QCategoryDesc = "People",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 6,
					QCategoryDesc = "System Operations and Maintenance",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 7,
					QCategoryDesc = "Data Structures",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionCategory()
				{
					Id = 8,
					QCategoryDesc = "Algorithms",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				});

			modelBuilder.Entity<QuestionType>()
				.HasData(
				new QuestionType()
				{
					Id = 1,
					QTypeDesc = "Multiple Choice",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionType()
				{
					Id = 2,
					QTypeDesc = "Multiple Choice + Audio",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionType()
				{
					Id = 3,
					QTypeDesc = "True or False",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				}, new QuestionType()
				{
					Id = 4,
					QTypeDesc = "Type Answer",
					DateCreated = DateTime.Now,
					UserId = admin.Id,
				});

		}

	}
}
