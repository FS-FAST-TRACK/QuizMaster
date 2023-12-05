using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.SeedData.Quiz;

namespace QuizMaster.API.QuizSession.DbContexts
{
	public class QuizSessionDbContext : DbContext
	{
		public DbSet<Question> Questions { get; set; }
		public DbSet<QuestionCategory> Categories { get; set; }
		public DbSet<QuestionDifficulty> Difficulties { get; set; }
		public DbSet<QuestionType> Types { get; set; }
		public DbSet<QuestionDetail> QuestionDetails { get; set; }
		public DbSet<QuestionDetailType> QuestionDetailTypes { get; set; }
		public DbSet<DetailType> DetailTypes { get; set; }
		public DbSet<QuizParticipant> QuizParticipants { get; set; }
		public DbSet<Set> Sets { get; set; }
		public DbSet<QuestionSet> QuestionSets { get; set; }
		public DbSet<QuizRoom> QuizRooms { get; set; }
		public DbSet<SetQuizRoom> SetQuizRooms { get; set; }


		public QuizSessionDbContext(DbContextOptions<QuizSessionDbContext> options) : base(options) { }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			// Disable cascade delete
			var cascadeFKs = modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.NoAction);

			foreach (var fk in cascadeFKs)
				fk.DeleteBehavior = DeleteBehavior.NoAction;

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
							.HasData(Library.Common.SeedData.Quiz.DetailTypes.SeedData);

		}
	}
}
