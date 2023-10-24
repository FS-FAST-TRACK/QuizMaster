using Microsoft.AspNetCore.Authentication.Cookies;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Authentication.Configuration;
using QuizMaster.API.Authentication.Helper;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Authentication.Services.Auth;
using QuizMaster.API.Authentication.Services.GRPC;
using QuizMaster.API.Authentication.Services.Temp;

namespace QuizMaster.API.Authentication
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

            // register the services
            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            // Configuring strongly typed settings object
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


            // configure cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>{
                    option.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = async (context) =>
                        {
                            // validate the cookie
                            await CookieHelper.ValidateCookie(context, builder.Configuration["AppSettings:JWTSecret"]);
                        }
                    };
                    option.ExpireTimeSpan = TimeSpan.FromHours(Convert.ToInt16(builder.Configuration["AppSettings:IntExpireHour"]));
                    option.SlidingExpiration = true; // renew cookie when it's about to expire,
                }) ;

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<Service>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.MapControllers();

            app.Run();
        }
    }
}