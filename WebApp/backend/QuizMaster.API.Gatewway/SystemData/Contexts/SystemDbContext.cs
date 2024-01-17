using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Gateway.Models.System;

namespace QuizMaster.API.Gateway.SystemData.Contexts
{
    public class SystemDbContext: DbContext
    {
        // Define the DbSet for the SystemDb Entities
        public DbSet<SystemAbout> SystemAboutData { get; set; }
        public DbSet<SystemContact> SystemContactData { get; set; }
        public DbSet<Reviews> SystemReviews { get; set; }
        public DbSet<ContactReaching> SystemReachingContacts { get; set; }

        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Initialize the defaults
            modelBuilder.Entity<SystemAbout>().HasData(SystemAbout.DEFAULT);
            modelBuilder.Entity<SystemContact>().HasData(SystemContact.DEFAULT);

            base.OnModelCreating(modelBuilder);
        }
    }
}
