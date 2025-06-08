using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// sætter maks size for pdf file uploade
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 50 MB fx
});

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

string host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "";
string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "";
string database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "";
string user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "";
string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";

string connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";

// ✅ Configure PostgreSQL with Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());

// for storing files on disk
builder.Services.AddScoped<IPdfStorageRepository, PdfStorageRepository>();
builder.Services.AddScoped<IPdfStorageService, PdfStorageService>();

// for storing meta data for files in db 
builder.Services.AddScoped<IPdfFileInfoRepository, PdfFileInfoRepository>();
builder.Services.AddScoped<IPdfFileInfoService, PdfFileInfoService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();