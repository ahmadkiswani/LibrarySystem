using LibrarySystem.UserIdentity.Data;
using LibrarySystem.UserIdentity.Iinterface;
using LibrarySystem.UserIdentity.Models;
using LibrarySystem.UserIdentity.Seed;
using LibrarySystem.UserIdentity.Services;
using LibrarySystem.Common.Middleware;
using LibrarySystem.Common.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("IdentityConnection")
    ));


builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

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


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
    );
});


var rabbitSection = builder.Configuration.GetSection("RabbitMq");
if (!rabbitSection.Exists())
    throw new Exception("RabbitMq configuration section is missing");

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitSection["Host"], "/", h =>
        {
            h.Username(rabbitSection["Username"]);
            h.Password(rabbitSection["Password"]);
        });

        cfg.Message<UserCreatedMessage>(m =>
        {
            m.SetEntityName(LibraryExchanges.Users);
        });

        cfg.Message<UserUpdatedMessage>(m =>
        {
            m.SetEntityName(LibraryExchanges.Users);
        });

        cfg.Message<UserDeactivatedMessage>(m =>
        {
            m.SetEntityName(LibraryExchanges.Users);
        });

        cfg.UseMessageRetry(r =>
            r.Interval(3, TimeSpan.FromSeconds(5))
        );
    });
});


var app = builder.Build();


app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>("UserIdentity.API");

app.UseAuthentication();

app.UseStatusCodePages(async context =>
{
    context.HttpContext.Response.ContentType = "application/json";
    await context.HttpContext.Response.WriteAsync(
        $"{{\"statusCode\":{context.HttpContext.Response.StatusCode}}}"
    );
});

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole<int>>>();

    await RoleSeeder.SeedAsync(roleManager);
}

app.Run();
