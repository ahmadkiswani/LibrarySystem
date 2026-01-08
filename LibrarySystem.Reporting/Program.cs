using LibrarySystem.Reporting.Consumers;
using LibrarySystem.Reporting.Settings;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<MongoSettings>(
            context.Configuration.GetSection("MongoSettings"));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return sp.GetRequiredService<IMongoClient>()
                     .GetDatabase(settings.DatabaseName);
        });

        services.AddMassTransit(x =>
        {
            x.AddConsumer<BorrowCreatedConsumer>();
            x.AddConsumer<BorrowReturnedConsumer>();
            x.AddConsumer<BorrowOverdueConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(context.Configuration["RabbitMq:Host"], h =>
                {
                    h.Username(context.Configuration["RabbitMq:Username"]);
                    h.Password(context.Configuration["RabbitMq:Password"]);
                });

                cfg.ReceiveEndpoint("borrow-created", e =>
                {
                    e.Bind("borrow.exchange", s =>
                    {
                        s.ExchangeType = "topic";
                        s.RoutingKey = "borrow.created";
                    });

                    e.ConfigureConsumer<BorrowCreatedConsumer>(ctx);
                });

                cfg.ReceiveEndpoint("borrow-returned", e =>
                {
                    e.Bind("borrow.exchange", s =>
                    {
                        s.ExchangeType = "topic";
                        s.RoutingKey = "borrow.returned";
                    });

                    e.ConfigureConsumer<BorrowReturnedConsumer>(ctx);
                });

                cfg.ReceiveEndpoint("borrow-overdue", e =>
                {
                    e.Bind("borrow.exchange", s =>
                    {
                        s.ExchangeType = "topic";
                        s.RoutingKey = "borrow.overdue";
                    });

                    e.ConfigureConsumer<BorrowOverdueConsumer>(ctx);
                });
            });
        });
    })
    .Build();

await host.RunAsync();
