using Application.Interfaces;
using Application.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);
builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<CaseService>();

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