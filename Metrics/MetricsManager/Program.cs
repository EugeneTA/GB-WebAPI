using MetricsManager.Converters;
using MetricsManager.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;

namespace MetricsManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure logging

            // NLog integration
            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();

            }).UseNLog(new NLogAspNetCoreOptions() { RemoveLoggerFactoryFilter = true });


            // Log HTTP requests
            // !!! Need to add  -> app.UseHttpLogging();

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.RequestHeaders.Add("Authorization");
                logging.RequestHeaders.Add("X-Real-IP");
                logging.RequestHeaders.Add("X-Forwarded-For");
            });

            #endregion

            // Add services to the container.

            builder.Services.AddSingleton<AgentPool>();

            builder.Services.AddControllers();

            // Add JSON option to convert TimeSpan
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsManager", Version = "v1" });

                // Поддержка TimeSpan
                c.MapType<TimeSpan>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            app.UseHttpLogging();

            app.Run();
        }
    }
}