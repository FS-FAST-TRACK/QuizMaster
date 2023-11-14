using Microsoft.EntityFrameworkCore;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Handlers;
using QuizMaster.API.QuizSession.Hubs;
using QuizMaster.API.QuizSession.Services.Repositories;

namespace QuizMaster.API.QuizSession
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

            // Enable SignalR
            builder.Services.AddSignalR();

            // Register Services
            builder.Services.AddSingleton<IChatRepository, SingletonChatRepository>(); // chat service repository
            builder.Services.AddSingleton<SessionHandler>(); // session handler service
            builder.Services.AddSingleton<SignalR_QuizSessionHub>(); // signalR hub service

            // Add DBcontext
			builder.Services.AddDbContext<QuizSessionDbContext>(
				dbContextOptions => dbContextOptions.UseSqlite(
					builder.Configuration["ConnectionStrings:QuizMasterQuizSessionDBConnectionString"]));


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting(); // required when use endpoints is implemented

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // quizsession signalR endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalR_QuizSessionHub>("/quizmaster_ws");
            });

            app.MapControllers();

            app.Run();
        }
    }
}