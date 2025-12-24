using LibrarySystem.Logging.Consumers;
using LibrarySystem.Logging.Services;
using LibrarySystem.Logging.Settings;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<MongoSettings>(
            context.Configuration.GetSection("MongoSettings"));

        services.AddSingleton<MongoLogService>();

        services.AddScoped<RequestResponseLogConsumer>();
        services.AddScoped<ExceptionLogConsumer>();
        services.AddHostedService<HostedService>();
    })
    .Build();

await host.RunAsync();
