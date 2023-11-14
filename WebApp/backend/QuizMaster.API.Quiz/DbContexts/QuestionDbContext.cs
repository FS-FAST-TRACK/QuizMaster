using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.DbContexts
{
	public class QuestionDbContext : DbContext
	{
		public DbSet<Question> Questions { get; set; }
		public DbSet<QuestionCategory> Categories { get; set; }
		public DbSet<QuestionDifficulty> Difficulties { get; set; }
		public DbSet<QuestionType> Types { get; set; }
		public DbSet<QuestionDetail> QuestionDetails { get; set; }
		public DbSet<QuestionDetailType> QuestionDetailTypes { get; set; }
		public DbSet<DetailType> DetailTypes { get; set; }
		public QuestionDbContext(DbContextOptions<QuestionDbContext> options) : base(options) { }

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

			// Seed Detail Types
			modelBuilder.Entity<DetailType>()
							.HasData(SeedData.DetailTypes.SeedData);

		}
	}
}
