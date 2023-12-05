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
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddGrpc();
			builder.Services.AddControllers();
            builder.Services.AddScoped(sp =>
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7065");
                return new AuditService.AuditServiceClient(channel);
            });
            // configure strongly typed app settings object
            builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
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

			// Register the worker services
			builder.Services.AddHostedService<RabbitMqUserWorker>();

            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            */
            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();


            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());
            app.UseAuthorization();

			app.MapGrpcService<InformationService>().RequireCors("AllowAll");
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapControllers();

			// Make sure database is created
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var dbContext = services.GetRequiredService<AccountDbContext>();

				// Ensure the database is created
				dbContext.Database.EnsureCreated();
			}


			app.Run();
		}
	}
}