using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using MonitoringService.DataAccess;
using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Monitoring.Services;
using QuizMaster.API.Monitoring.Services.Audit;
using QuizMaster.API.Monitoring.Services.QuizAudit;

namespace QuizMaster.API.Monitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserAuditTrailRepository, UserAuditTrailRepository>();
            builder.Services.AddScoped<IUserAuditTrailService, UserAuditTrailService>();
            builder.Services.AddScoped<UserAuditTrailRepository>();
            builder.Services.AddScoped<IQuizCategoryAuditTrailRepository, QuizCategoryAuditTrailRepository>();
            builder.Services.AddScoped<IQuizCategoryAuditTrailService, QuizCategoryAuditTrailService>();
            builder.Services.AddScoped<QuizCategoryAuditTrailRepository>();
            builder.Services.AddScoped<IQuizDifficultyAuditTrailRepository, QuizDifficultyAuditTrailRepository>();
            builder.Services.AddScoped<IQuizDifficultyAuditTrailService, QuizDifficultyAuditTrailService>();
            builder.Services.AddScoped<QuizDifficultyAuditTrailRepository>();

            builder.Services.AddGrpc();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //add console logging
            builder.Services.AddLogging();
            //add debug output
            builder.Logging.AddDebug();
            builder.Services.AddDbContext<MonitoringDbContext>(
                 dbContextOptions => dbContextOptions.UseSqlServer(
                     builder.Configuration["ConnectionStrings:AuditTrailDBConnection"]));



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Logging.ClearProviders(); // Remove the default logging providers
            builder.Logging.AddConsole(); // Add the console logger
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapGrpcService<MonitoringInfoService>();
            app.MapGrpcService<QuizMonitoringService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapControllers();

            app.Run();
        }
    }
}