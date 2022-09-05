using MetricsAgent.Services.Implemetation;
using MetricsAgent.Services;
using MetricsAgent.Converters;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Data.SQLite;
using MetricsAgent.Options;
using AutoMapper;

namespace MetricsAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Automapper

            // Create mapper configuration from our class MapperProfile
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));

            // Create mapper from our configuration
            var mapper = mapperConfiguration.CreateMapper();
            
            // Register our mapper with the app as singletone 
            builder.Services.AddSingleton(mapper);

            #endregion

            #region Configure Repository

            builder.Services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            builder.Services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
            builder.Services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
            builder.Services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            builder.Services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();

            #endregion

            #region Configure Database

            #region DatabaseOption

            builder.Services.Configure<DatabaseOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:DatabaseOptions").Bind(options);
            });

            #endregion

            #region CreateDatabase
            //ConfigureSqlLiteConnection(builder.Services);
            #endregion

            #endregion

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
            builder.Services.AddControllers();

            // Add JSON option to convert TimeSpan
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });

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

            // Enable HTTP requests Log
            app.UseHttpLogging();

            app.MapControllers();

            app.Run();
        }


        private static void ConfigureSqlLiteConnection(WebApplicationBuilder applicationBuilder)
        {

            //const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100;";
            //var connection = new SQLiteConnection(connectionString);
            //connection.Open();
            //PrepareSchema(connection
            //

            var connection = new SQLiteConnection(applicationBuilder.Configuration["Settings:DatabaseOptions:ConnectionString"].ToString());
            connection.Open();
            PrepareSchema(connection);
        }

        private static void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();
                command.CommandText =
                    @"CREATE TABLE cpumetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();
                command.CommandText =
                    @"CREATE TABLE dotnetmetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();
                command.CommandText =
                    @"CREATE TABLE hddmetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS networkmetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();
                command.CommandText =
                    @"CREATE TABLE networkmetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(connection))
            {
                // Задаём новый текст команды для выполнения
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS rammetrics";
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery();
                command.CommandText =
                    @"CREATE TABLE rammetrics(id INTEGER
                    PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
            }
        }
    }
}