using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.QuizSession.Configuration;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Handlers;
using QuizMaster.API.QuizSession.Hubs;
using QuizMaster.API.QuizSession.Services.Grpc;
using QuizMaster.API.QuizSession.Services.Repositories;
using QuizMaster.API.QuizSession.Services.Workers;

namespace QuizMaster.API.QuizSession
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
            builder.Services.AddScoped(sp =>
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7065");
                return new RoomAuditService.RoomAuditServiceClient(channel);
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Enable logging
            builder.Services.AddLogging();

            // Enable strongly typed settings object
            builder.Services.Configure<QuizSessionApplicationSettings>(builder.Configuration.GetSection("AppSettings"));

            // Enable SignalR
            builder.Services.AddSignalR();

            // Register Services
            builder.Services.AddSingleton<IChatRepository, SingletonChatRepository>(); // chat service repository
            builder.Services.AddSingleton<SessionHandler>(); // session handler service
            builder.Services.AddSingleton<SignalR_QuizSessionHub>(); // signalR hub service
            builder.Services.AddScoped<IQuizSessionRepository, QuizSessionRepository>(); // quiz session repository

            // Add DBcontext
			builder.Services.AddDbContext<QuizSessionDbContext>(
				dbContextOptions => dbContextOptions.UseSqlite(
					builder.Configuration["ConnectionStrings:QuizMasterQuizSessionDBConnectionString"]));

            // Register worker services
            builder.Services.AddHostedService<QuestionSynchronizationWorkerService>();

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
                
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting(); // required when use endpoints is implemented

            app.UseHttpsRedirection();
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());
            app.UseAuthorization();

            // quizsession signalR endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalR_QuizSessionHub>("/quizmaster_ws");
            });
            app.MapGrpcService<QuizSetServices>().RequireCors("AllowAll"); ;
            app.MapGrpcService<QuizRoomServices>().RequireCors("AllowAll"); ;
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapControllers();

            using(var scope = app.Services.CreateScope())
            {
                bool run = false;
                while (!run)
                {
                    try
                    {
                        Task.Delay(1000).Wait();
                        var scopeProvider = scope.ServiceProvider;
                        var dbContext = scopeProvider.GetRequiredService<QuizSessionDbContext>();

                        dbContext.Database.Migrate();
                        dbContext.Database.EnsureCreated();
                        run = true;
                    }
                    catch { }
                }
            }

            app.Run();
        }
    }
}