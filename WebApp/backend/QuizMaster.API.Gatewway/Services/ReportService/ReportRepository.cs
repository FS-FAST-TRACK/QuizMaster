using QuizMaster.API.Gateway.Models.Report;
using QuizMaster.API.Gateway.SystemData.Contexts;
using System.Collections.Immutable;

namespace QuizMaster.API.Gateway.Services.ReportService
{
    public class ReportRepository
    {
        private readonly SystemDbContext dbContext;

        public ReportRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SaveReport(QuizReport report)
        {
            dbContext.QuizReports.Add(report);
            dbContext.SaveChanges();
        }

        public IEnumerable<QuizReport> GetQuizReports()
        {
            return dbContext.QuizReports.ToImmutableList();
        }
    }
}
