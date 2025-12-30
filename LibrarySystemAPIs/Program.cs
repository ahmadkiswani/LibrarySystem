using LibrarySystem.Common.Messaging;
using LibrarySystem.Domain.Data;
using LibrarySystem.Domain.Repositories;
using LibrarySystem.Services;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.HelperDto;
using LibrarySystem.Common.Middleware;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors?.Count > 0)
                .SelectMany(e => e.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return new BadRequestObjectResult(
                new BaseResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = errors
                });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin());
});


var rabbitSection = builder.Configuration.GetSection("RabbitMq");
if (!rabbitSection.Exists())
    throw new Exception("RabbitMq configuration section is missing");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<UserUpdatedConsumer>();
    x.AddConsumer<UserDeactivatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitSection["Host"], "/", h =>
        {
            h.Username(rabbitSection["Username"]);
            h.Password(rabbitSection["Password"]);

        });

        cfg.ReceiveEndpoint(LibraryQueues.UserCreated, e =>
        {
            e.Bind(LibraryExchanges.Users, s =>
            {
                s.RoutingKey = LibraryRoutingKeys.UserCreated;
                s.ExchangeType = "topic";
            });

            e.ConfigureConsumer<UserCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint(LibraryQueues.UserUpdated, e =>
        {
            e.Bind(LibraryExchanges.Users, s =>
            {
                s.RoutingKey = LibraryRoutingKeys.UserUpdated;
                s.ExchangeType = "topic";
            });

            e.ConfigureConsumer<UserUpdatedConsumer>(context);
        });

        cfg.ReceiveEndpoint(LibraryQueues.UserDeactivated, e =>
        {
            e.Bind(LibraryExchanges.Users, s =>
            {
                s.RoutingKey = LibraryRoutingKeys.UserDeactivated;
                s.ExchangeType = "topic";
            });

            e.ConfigureConsumer<UserDeactivatedConsumer>(context);
        });
    });
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookCopyService, BookCopyService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>("LibrarySystem.API");

app.MapControllers();
app.Run();
