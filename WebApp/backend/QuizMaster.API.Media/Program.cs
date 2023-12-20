using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Media.Data.Context;
using QuizMaster.API.Media.Services;
using QuizMaster.API.Media.Services.GRPC;
using QuizMaster.API.Monitoring.Proto;

namespace QuizMaster.API.Media
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
                return new MediaAuditService.MediaAuditServiceClient(channel);
            });
            builder.Services.AddControllers();
            builder.Services.AddLogging();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DbContext
            builder.Services.AddDbContext<FileDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnStr")));

            // register services
            builder.Services.AddScoped<IFileRepository, FileRepository>();

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
            }

            // app.UseHttpsRedirection();
            // add the cors policy to the app
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowCredentials().AllowAnyHeader());

            app.UseAuthorization();

            app.MapGrpcService<Service>().RequireCors("AllowAll"); ;
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapControllers();

            // Make sure database is created
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                bool run = false;
                while (!run)
                {
                    try
                    {
                        Task.Delay(1000).Wait();
                        var dbContext = services.GetRequiredService<FileDbContext>();
                        // Ensure the database is created
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