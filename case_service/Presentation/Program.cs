using Application.Domain.Messages;
using Application.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using EasyNetQ;
using Infrastructure.Contexts;
using Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Presentation.Messages;
using Presentation.Messages.options;

;


var builder = WebApplication.CreateBuilder(args);

// ✅ Load environment variables from .env
if (File.Exists("../../.env"))
{
    foreach (var line in File.ReadAllLines("../.env"))
    {
        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
            continue;

        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

// ✅ Get PostgreSQL connection info from environment
string host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "";
string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
string database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "";
string user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "";
string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";

string connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";



// ✅ Configure PostgreSQL with Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<IUserOnCaseRepository, EFUserOnCaseRepository>();
builder.Services.AddScoped<IUserOnCaseService, UserOnCaseService>();

// Registrer repositories og services
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Dependency injection
builder.Services.AddScoped<ICaseRepository, EFCaseRepository>();
builder.Services.AddScoped<ICaseService, CaseService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMqOptions>(opts =>
{
    opts.Host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    opts.User = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
    opts.Pass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
});

// Registrér IBus som singleton, afhængig af RabbitMqOptions 
builder.Services.AddSingleton<IBus>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    return RabbitHutch.CreateBus(options.ConnectionString);
});

// rabbitmq listeners
builder.Services.AddHostedService<FileCreatedMessageHandler>();
builder.Services.AddHostedService<FileDeletedMessageHandler>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();