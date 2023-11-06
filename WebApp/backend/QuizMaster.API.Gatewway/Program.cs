using Microsoft.AspNetCore.Authentication.Cookies;
using QuizMaster.API.Authentication.Configuration;
using QuizMaster.API.Authentication.Helper;
using QuizMaster.API.Authentication.Services;
using QuizMaster.API.Authentication.Services.Auth;
using QuizMaster.API.Authentication.Services.GRPC;
using QuizMaster.API.Authentication.Services.Temp;
using QuizMaster.API.Authentication.Services.Worker;
using QuizMaster.API.Gateway.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Inject Auto Mapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();

// Configuring strongly typed settings object
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<GrpcServerConfiguration>(builder.Configuration.GetSection("GrpcServerConfiguration"));

// register the services
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<RabbitMqUserWorker>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddSingleton<RabbitMqRepository>();


// configure cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
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
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
