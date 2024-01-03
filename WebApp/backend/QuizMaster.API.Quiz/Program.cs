using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Monitoring.DataAccess;
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
				var handler = new HttpClientHandler();
				handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

				var channel = GrpcChannel.ForAddress(builder.Configuration["AppSettings:Service_MonitoringGRPC"], new GrpcChannelOptions { HttpHandler = handler});
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

            // Register worker service
            builder.Services.AddScoped<QuizDataSynchronizationWorker>(); 
			builder.Services.AddHostedService<QuizDataSynchronizationWorker>();

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


			app.MapGrpcService<Service>().RequireCors("AllowAll"); ;
			app.MapGrpcService<QuestionService>().RequireCors("AllowAll"); ;
			app.MapGrpcService<DifficultyService>().RequireCors("AllowAll"); ;
			app.MapGrpcService<TypeService>().RequireCors("AllowAll"); ;
			app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

			app.MapControllers();

			Console.WriteLine("Starting Service");
			//Make sure database is created

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
						var dbContext = services.GetRequiredService<QuestionDbContext>();
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