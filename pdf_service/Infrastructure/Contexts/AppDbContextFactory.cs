using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Infrastructure.Contexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
            var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "your_db";
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "your_user";
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "your_pass";
            string port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5431";


            string connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
