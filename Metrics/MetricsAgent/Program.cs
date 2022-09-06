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
using FluentMigrator.Runner;
using MetricsAgent.Job;
using MetricsAgent.Jobs;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;

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

            #region Fluent Migrator

            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(options =>
                // ��������� ��������� SQLite
                options.AddSQLite()
                // ������������� ������ �����������
                .WithGlobalConnectionString(builder.Configuration["Settings:DatabaseOptions:ConnectionString"].ToString())
                // ������������, ��� ������ ������ � ����������
                .ScanIn(typeof(Program).Assembly).For.Migrations()
                ).AddLogging(lb => lb.AddFluentMigratorConsole());

            #endregion

            #region CreateDatabase (Old version)
            //ConfigureSqlLiteConnection(builder.Services);
            #endregion

            #endregion

            #region Schedule/Jobs (Quartz)

            #region Job Options
            
            // Job option from app settings
            builder.Services.Configure<CpuJobOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:MetricsOptions:CpuJobOptions").Bind(options);
            });

            builder.Services.Configure<DotNetJobOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:MetricsOptions:DotNetJobOptions").Bind(options);
            });

            builder.Services.Configure<HddJobOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:MetricsOptions:HddJobOptions").Bind(options);
            });

            builder.Services.Configure<NetworkJobOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:MetricsOptions:NetworkJobOptions").Bind(options);
            });

            builder.Services.Configure<RamJobOptions>(options =>
            {
                builder.Configuration.GetSection("Settings:MetricsOptions:RamJobOptions").Bind(options);
            });

            #endregion

            // ����������� ������� �������
            builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();

            // ����������� �������� ������� Quartz
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // ����������� ������� ����� ������
            builder.Services.AddSingleton<CpuMetricJob>();
            builder.Services.AddSingleton<DotNetMetricJob>();
            builder.Services.AddSingleton<HddMetricJob>();
            builder.Services.AddSingleton<NetworkMetricJob>();
            builder.Services.AddSingleton<RamMetricJob>();

            // ����������� ������������
            builder.Services.AddHostedService<QuartzHostedService>();

            // ���� � ������� ����������� ���������� cron ������
            // https://www.freeformatter.com/cron-expression-generator-quartz.html

            // ���������� ���������� ������ �������� �� ����� ������
            builder.Services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(DotNetMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(HddMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(NetworkMetricJob), "0/5 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(RamMetricJob), "0/5 * * ? * * *"));

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

                // ��������� TimeSpan
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

            #region Fluent Migration Run

            var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope serviceScope = serviceScopeFactory.CreateScope())
            {
                var migrationRunner = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                migrationRunner.MigrateUp();
            }

            #endregion

            app.Run();
        }


        //private static void ConfigureSqlLiteConnection(WebApplicationBuilder applicationBuilder)
        //{

        //    //const string connectionString = "Data Source = metrics.db; Version = 3; Pooling = true; Max Pool Size = 100;";
        //    //var connection = new SQLiteConnection(connectionString);
        //    //connection.Open();
        //    //PrepareSchema(connection
        //    //

        //    var connection = new SQLiteConnection(applicationBuilder.Configuration["Settings:DatabaseOptions:ConnectionString"].ToString());
        //    connection.Open();
        //    PrepareSchema(connection);
        //}

        //private static void PrepareSchema(SQLiteConnection connection)
        //{
        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        // ����� ����� ����� ������� ��� ����������
        //        // ������� ������� � ���������, ���� ��� ���� � ���� ������
        //        command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
        //        // ���������� ������ � ���� ������
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE cpumetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }

        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        // ����� ����� ����� ������� ��� ����������
        //        // ������� ������� � ���������, ���� ��� ���� � ���� ������
        //        command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";
        //        // ���������� ������ � ���� ������
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE dotnetmetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }

        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        // ����� ����� ����� ������� ��� ����������
        //        // ������� ������� � ���������, ���� ��� ���� � ���� ������
        //        command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
        //        // ���������� ������ � ���� ������
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE hddmetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }

        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        // ����� ����� ����� ������� ��� ����������
        //        // ������� ������� � ���������, ���� ��� ���� � ���� ������
        //        command.CommandText = "DROP TABLE IF EXISTS networkmetrics";
        //        // ���������� ������ � ���� ������
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE networkmetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }

        //    using (var command = new SQLiteCommand(connection))
        //    {
        //        // ����� ����� ����� ������� ��� ����������
        //        // ������� ������� � ���������, ���� ��� ���� � ���� ������
        //        command.CommandText = "DROP TABLE IF EXISTS rammetrics";
        //        // ���������� ������ � ���� ������
        //        command.ExecuteNonQuery();
        //        command.CommandText =
        //            @"CREATE TABLE rammetrics(id INTEGER
        //            PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();
        //    }
        //}
    }
}