using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Quiz.DbContexts;
using QuizMaster.API.Quiz.Services.Repositories;

namespace QuizMaster.API.Quiz
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<QuestionDbContext>(
				dbContextOptions => dbContextOptions.UseSqlServer(
					builder.Configuration["ConnectionStrings:QuizMasterQuestionDBConnectionString"]));


			builder.Services.AddScoped<IQuizRepository, QuizRepository>();

			builder.Services.AddLogging();

			builder.Services.AddControllers().AddNewtonsoftJson();

			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}