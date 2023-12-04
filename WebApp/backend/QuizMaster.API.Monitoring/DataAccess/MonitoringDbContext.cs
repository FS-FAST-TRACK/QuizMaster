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

            base.OnModelCreating(modelBuilder);
        }
    }
}
