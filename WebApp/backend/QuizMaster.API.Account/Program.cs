using Grpc.Net.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Account.Configuration;
using QuizMaster.API.Account.DbContext;
using QuizMaster.API.Account.Service;
using QuizMaster.API.Account.Service.Worker;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Roles;

namespace QuizMaster.API.Account
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Starting Account API");
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddGrpc();
			builder.Services.AddControllers();
            builder.Services.AddScoped(sp =>
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                var channel = GrpcChannel.ForAddress(builder.Configuration["ApplicationSettings:Service_MonitoringGRPC"], new GrpcChannelOptions { HttpHandler = handler });
                return new AuditService.AuditServiceClient(channel);
            });
			
			builder.Services.AddLogging();
            // configure strongly typed app settings object
            builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
            builder.Services.AddScoped<EmailSenderService>();
			builder.Services.AddSingleton<PasswordHandler>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<AccountDbContext>(
				dbContextOptions => dbContextOptions.UseSqlServer(
					builder.Configuration["ConnectionStrings:QuizMasterAccountDBConnectionString"]));
			
			builder.Services.AddIdentity<UserAccount, UserRole>()
					.AddEntityFrameworkStores<AccountDbContext>()
					.AddDefaultTokenProviders();

			builder.Services.AddControllers().AddNewtonsoftJson();

			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));


            // Register the worker services
            builder.Services.AddHostedService<RabbitMqUserWorker>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				//app.UseSwagger();
				//app.UseSwaggerUI();
			}

            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());

            app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapGrpcService<InformationService>().RequireCors("AllowAll"); ;
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
						var dbContext = services.GetRequiredService<AccountDbContext>();
						// Ensure the database is created
						dbContext.Database.EnsureCreated();
                        Console.WriteLine("Database was built successfully");
                        run = true;
					}
					catch { }
				}
			}


			app.Run();
		}
	}
}