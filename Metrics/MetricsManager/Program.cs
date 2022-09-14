using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.Converters;
using MetricsManager.Models;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Options;
using MetricsManager.Services;
using MetricsManager.Services.Client;
using MetricsManager.Services.Client.Implementation;
using MetricsManager.Services.Implementation;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Polly;
using Polly.Extensions.Http;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using MetricsManager.Jobs;

namespace MetricsManager
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

            builder.Services.AddScoped<IMetricAgentRepository, MetricAgentRepository>();
            //builder.Services.AddSingleton<AgentPool>();

            #region Database

            // Database options
            builder.Services.Configure<DatabaseOptions>(options => builder.Configuration.GetSection("Settings:DatabaseOptions").Bind(options));

            // Add Fluent Migrator
            builder.Services.AddFluentMigratorCore().
                ConfigureRunner(options =>
                options.AddSQLite().
                WithGlobalConnectionString(builder.Configuration["Settings:DatabaseOptions:ConnectionString"])
                .ScanIn(typeof(Program).Assembly).For.Migrations()
                ).AddLogging(lb => lb.AddFluentMigratorConsole());

            #endregion

            #region Configure HTTP Client

            //builder.Services.AddHttpClient();

            //builder.Services.AddHttpClient<ICpuMetricsAgentClient, CpuMetricsAgentClient>()
            //    .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
            //    .AddPolicyHandler(GetRetryPolicy());

            //static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            //{
            //    return HttpPolicyExtensions
            //        .HandleTransientHttpError()
            //        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            //        .WaitAndRetryAsync(100, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)));
            //}

            builder.Services.AddHttpClient<ICpuMetricsAgentClient, CpuMetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
                onRetry: (response, sleepDuration, attemptCount, context) =>
                {
                    var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                    logger.LogError(response.Exception != null ? response.Exception :
                        new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"),
                        $"(attempt: {attemptCount}) request exception.");
                }
                ));

            builder.Services.AddHttpClient<IDotNetMetricsAgentClient, DotNetMetricsAgentClient>()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
                    onRetry: (response, sleepDuration, attemptCount, context) =>
                    {
                        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                        logger.LogError(response.Exception != null ? response.Exception :
                            new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"),
                            $"(attempt: {attemptCount}) request exception.");
                    }
                    ));

            builder.Services.AddHttpClient<IHddMetricsAgentClient, HddMetricsAgentClient>()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
                    onRetry: (response, sleepDuration, attemptCount, context) =>
                    {
                        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                        logger.LogError(response.Exception != null ? response.Exception :
                            new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"),
                            $"(attempt: {attemptCount}) request exception.");
                    }
                    ));

            builder.Services.AddHttpClient<INetworkMetricsAgentClient, NetworkMetricsAgentClient>()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
                    onRetry: (response, sleepDuration, attemptCount, context) =>
                    {
                        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                        logger.LogError(response.Exception != null ? response.Exception :
                            new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"),
                            $"(attempt: {attemptCount}) request exception.");
                    }
                    ));

            builder.Services.AddHttpClient<IRamMetricsAgentClient, RamMetricsAgentClient>()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * 2),
                    onRetry: (response, sleepDuration, attemptCount, context) =>
                    {
                        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
                        logger.LogError(response.Exception != null ? response.Exception :
                            new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}"),
                            $"(attempt: {attemptCount}) request exception.");
                    }
                    ));


            //builder.Services.AddHttpClient<ICpuMetricsAgentClient, CpuMetricsAgentClient>()
            //    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(
            //        retryCount: 3,
            //        sleepDurationProvider: (attemptcount) => TimeSpan.FromSeconds(attemptcount * 2),
            //        onRetry: (response, sleepDuration, attemptCount, context) =>
            //        {
            //            var logger = builder.Services.BuildServiceProvider().GetService<ILogger<Program>>();
            //            logger.LogError(response.Exception != null ? response.Exception :
            //                new Exception($"\n {response.Result.StatusCode}: {response.Result.RequestMessage}"),
            //                $"(attempt: {attemptCount}) request exception");
            //        }
            //        ));

            #endregion

            #region Schedule/Jobs (Quartz)

            // Регистрация сервиса фабрики
            builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();

            // Регистрация базового сервиса Quartz
            builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Регистрация сервиса самой задачи
            builder.Services.AddSingleton<CpuMetricJob>();
            builder.Services.AddSingleton<DotNetMetricJob>();
            builder.Services.AddSingleton<HddMetricJob>();
            builder.Services.AddSingleton<NetworkMetricJob>();
            builder.Services.AddSingleton<RamMetricJob>();

            // Регистрация планировщика
            builder.Services.AddHostedService<QuartzHostedService>();

            // Сайт с удобным интерфейсом построения cron строки
            // https://www.freeformatter.com/cron-expression-generator-quartz.html

            // Добавление расписания работы сервисов по сбору данных
            builder.Services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/10 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(DotNetMetricJob), "0/11 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(HddMetricJob), "0/12 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(NetworkMetricJob), "0/13 * * ? * * *"));
            builder.Services.AddSingleton(new JobSchedule(typeof(RamMetricJob), "0/14 * * ? * * *"));

            #endregion

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
    }
}