using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.SeedData;

namespace QuizMaster.API.QuizSession.DbContexts
{
	public class QuizSessionDbContext : DbContext
	{
		public DbSet<Question> Questions { get; set; }
		public DbSet<QuestionCategory> Categories { get; set; }
		public DbSet<QuestionDifficulty> Difficulties { get; set; }
		public DbSet<QuestionType> Types { get; set; }
		public DbSet<QuestionDetail> Details { get; set; }
		public QuizSessionDbContext(DbContextOptions<QuizSessionDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			// Seed Question Types
			modelBuilder.Entity<QuestionType>()
							.HasData(QuestionTypes.Types);

			// Seed Question Difficulty
			modelBuilder.Entity<QuestionDifficulty>()
							.HasData(QuestionDifficulties.Difficulties);

			// Seed Question Category
			modelBuilder.Entity<QuestionCategory>()
							.HasData(QuestionCategories.Categories);

		}
	}
}
