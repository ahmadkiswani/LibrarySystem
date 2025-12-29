using LibrarySystem.Common.LibraryQueues;
using LibrarySystem.Logging.Consumers;
using LibrarySystem.Logging.Services;
using LibrarySystem.Logging.Settings;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<MongoSettings>(
            context.Configuration.GetSection("MongoSettings"));

        services.AddSingleton<MongoLogService>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<LogRequestConsumer>();
            x.AddConsumer<LogResponseConsumer>();
            x.AddConsumer<LogExceptionConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(LoggingQueues.RequestResponse, e =>
                {
                    e.ConfigureConsumer<LogRequestConsumer>(ctx);
                    e.ConfigureConsumer<LogResponseConsumer>(ctx);
                });

                cfg.ReceiveEndpoint(LoggingQueues.Exceptions, e =>
                {
                    e.ConfigureConsumer<LogExceptionConsumer>(ctx);
                });
            });
        });
    })
    .Build();

await host.RunAsync();
