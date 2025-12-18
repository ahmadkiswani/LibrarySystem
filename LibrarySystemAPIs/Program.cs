using LibrarySystem.API.Middleware;
using LibrarySystem.Domain.Data;
using LibrarySystem.Domain.Repositories;
using LibrarySystem.Logging.Consumers;
using LibrarySystem.Logging.Interfaces;
using LibrarySystem.Logging.Services;
using LibrarySystem.Logging.Settings;
using LibrarySystem.Services;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.HelperDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(e => e.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var response = new BaseResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
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
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<ILogService, RabbitMqLogService>();
builder.Services.AddSingleton<MongoLogService>();
builder.Services.AddSingleton<RequestResponseLogConsumer>();
builder.Services.AddSingleton<ExceptionLogConsumer>();
builder.Services.AddHostedService<LoggingHostedService>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();


app.Run();
