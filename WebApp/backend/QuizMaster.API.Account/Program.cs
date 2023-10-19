using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Account.DbContext;
using QuizMaster.API.Account.Service;
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

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapGrpcService<InformationService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapControllers();

			app.Run();
		}
	}
}