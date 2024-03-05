using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using MonitoringService.DataAccess;
using QuizMaster.API.Monitoring.DataAccess;
using QuizMaster.API.Monitoring.DataAccess.Media;
using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.API.Monitoring.DataAccess.Room;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Monitoring.Services;
using QuizMaster.API.Monitoring.Services.Audit;
using QuizMaster.API.Monitoring.Services.Media;
using QuizMaster.API.Monitoring.Services.QuizAudit;
using QuizMaster.API.Monitoring.Services.Room;

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
            builder.Services.AddScoped<IQuestionAuditTrailRepository, QuestionAuditTrailRepository>();
            builder.Services.AddScoped<IQuestionAuditTrailService, QuestionAuditTrailService>();
            builder.Services.AddScoped<QuestionAuditTrailRepository>();
            builder.Services.AddScoped<IQuestionTypeAuditTrailRepository, QuestionTypeAuditTrailRepository>();
            builder.Services.AddScoped<IQuestionTypeAuditTrailService, QuestionTypeAuditTrailService>();
            builder.Services.AddScoped<QuestionTypeAuditTrailRepository>();
            builder.Services.AddScoped<IMediaAuditTrailRepository, MediaAuditTrailRepository>();
            builder.Services.AddScoped<IMediaAuditTrailService, MediaAuditTrailService>();
            builder.Services.AddScoped<MediaAuditTrailRepository>();
            builder.Services.AddScoped<IQuizSetAuditTrailRepository, QuizSetAuditTrailRepository>();
            builder.Services.AddScoped<IQuizSetAuditTrailService, QuizSetAuditTrailService>();
            builder.Services.AddScoped<QuizSetAuditTrailRepository>();
            builder.Services.AddScoped<IRoomAuditTrailRepository, RoomAuditTrailRepository>();
            builder.Services.AddScoped<IRoomAuditTrailService, RoomAuditTrailService>();
            builder.Services.AddScoped<RoomAuditTrailRepository>();

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

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(options => options.SetIsOriginAllowed(x => true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());

            app.UseAuthorization();
            app.MapGrpcService<MonitoringInfoService>().RequireCors("AllowAll"); ;
            app.MapGrpcService<QuizMonitoringService>().RequireCors("AllowAll"); ;
            app.MapGrpcService<MediaMonitoringInfoService>().RequireCors("AllowAll"); ;
            app.MapGrpcService<RoomMonitoringInfoService>().RequireCors("AllowAll");
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapControllers();

            // Make sure database is created
            using (var scope = app.Services.CreateScope())
            {
                bool run = false;
                while (!run)
                {
                    try
                    {
                        Console.WriteLine("Building DB");
                        var services = scope.ServiceProvider;
                        Task.Delay(1000).Wait();
                        var dbContext = services.GetRequiredService<MonitoringDbContext>();
                        // Ensure the database is created
                        dbContext.Database.EnsureCreated();
                        Console.WriteLine("DB Was built successfully");
                        run = true;
                    }
                    catch { }
                }
            }

            app.Run();
        }
    }
}