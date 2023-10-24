using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Media.Models;

namespace QuizMaster.API.Media.Data.Context
{
    public class FileDbContext: DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { }

        public DbSet<FileInformation> Files { get; set; }
    }
}
