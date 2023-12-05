using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Configuration;
using QuizMaster.API.Quiz.DbContexts;
using QuizMaster.API.Quiz.Services;
using QuizMaster.API.Quiz.Services.GRPC;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.API.Quiz.Services.Workers;

namespace QuizMaster.API.Quiz
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddGrpc();
            builder.Services.AddScoped(sp =>
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7065");
                return new QuizAuditService.QuizAuditServiceClient(channel);
            });
            builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<QuestionDbContext>(
				dbContextOptions => dbContextOptions.UseSqlServer(
					builder.Configuration["ConnectionStrings:QuizMasterQuestionDBConnectionString"]));

			// Configure strongly typed app settings object
			builder.Services.Configure<QuizApplicationSettings>(builder.Configuration.GetSection("AppSettings"));


			builder.Services.AddScoped<IQuizRepository, QuizRepository>();

			builder.Services.AddScoped<IQuestionDetailManager, QuestionDetailManager>();

			builder.Services.AddLogging();

			builder.Services.AddControllers().AddNewtonsoftJson();

			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			// Register worker services
			builder.Services.AddHostedService<QuizDataSynchronizationWorker>();


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();

				// add the cors policy to the app
				// allow any origin to access api endpoints on development mode
				
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

	//		app.UseCors(p => p.WithOrigins("http://localhost:3000")
	//.AllowAnyHeader()
	//.AllowAnyMethod().WithHeaders());
			app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());


			app.MapGrpcService<Service>();
			app.MapGrpcService<QuestionService>();
			app.MapGrpcService<DifficultyService>();
			app.MapGrpcService<TypeService>();
			app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

			app.MapControllers();

			// Make sure database is created
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var dbContext = services.GetRequiredService<QuestionDbContext>();

				// Ensure the database is created
				dbContext.Database.EnsureCreated();
			}

			app.Run();
		}
	}
}