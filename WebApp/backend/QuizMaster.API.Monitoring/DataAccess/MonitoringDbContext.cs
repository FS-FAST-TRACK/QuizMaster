using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Audits;

namespace QuizMaster.API.Monitoring.DataAccess
{
    public class MonitoringDbContext : DbContext
    {
        public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options) : base(options)
        {
        }

        // Define DbSet for the AuditTrail entity
        public DbSet<UserAuditTrail> UserAuditTrails { get; set; }
        public DbSet<QuizAuditTrail> QuizAuditTrails { get; set; }
        public DbSet<QuizDifficultyAuditTrail> QuizDifficultyAuditTrails { get; set; }
        public DbSet<QuestionAuditTrail> QuestionAuditTrails { get; set; }
        public DbSet<QuestionTypeAuditTrail> QuestionTypeAuditTrails { get; set; }
        public DbSet<MediaAuditTrail> MediaAuditTrails { get; set; }
        public DbSet<QuizSetAuditTrail> QuizSetAuditTrails { get; set; }
        public DbSet<RoomAuditTrail> RoomAuditTrails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the AuditTrail entity, including its properties
            modelBuilder.Entity<UserAuditTrail>().HasKey(a => a.UserAuditTrailId);
            modelBuilder.Entity<UserAuditTrail>().Property(a => a.UserId).IsRequired();
            modelBuilder.Entity<UserAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<UserAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<UserAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<QuizAuditTrail>().HasKey(a => a.QuizAuditTrailId);
            modelBuilder.Entity<QuizAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<QuizAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<QuizAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<QuizDifficultyAuditTrail>().HasKey(a => a.QuizDiffAuditTrailId);
            modelBuilder.Entity<QuizDifficultyAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<QuizDifficultyAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<QuizDifficultyAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<QuestionAuditTrail>().HasKey(a => a.QuestionAuditTrailId);
            modelBuilder.Entity<QuestionAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<QuestionAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<QuestionAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<QuestionTypeAuditTrail>().HasKey(a => a.QuestionTypeAuditTrailId);
            modelBuilder.Entity<QuestionTypeAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<QuestionTypeAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<QuestionTypeAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<MediaAuditTrail>().HasKey(a => a.MediaAuditTrailId);
            modelBuilder.Entity<MediaAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<MediaAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<MediaAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<QuizSetAuditTrail>().HasKey(a => a.QuizSetAuditTrailId);
            modelBuilder.Entity<QuizSetAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<QuizSetAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<QuizSetAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            modelBuilder.Entity<RoomAuditTrail>().HasKey(a => a.RoomAuditTrailId);
            modelBuilder.Entity<RoomAuditTrail>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<RoomAuditTrail>().Property(a => a.Timestamp).IsRequired();
            modelBuilder.Entity<RoomAuditTrail>().Property(a => a.Details).HasMaxLength(255);

            base.OnModelCreating(modelBuilder);
        }
    }
}
