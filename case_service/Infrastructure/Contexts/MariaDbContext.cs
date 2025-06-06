// Fil: Infrastructure/Contexts/AppDbContext.cs
using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Contexts
{
    public class AppDbContext : DbContext, IDbContext
    {
        public DbSet<Case> Cases { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbConnection GetDbConnection() => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>(entity =>
            {
                entity.ToTable("case_info");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CaseNumber).HasMaxLength(50);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                
                // Dette tilføjer en global filter: alle queries på Case vil automatisk ignorere dem med DeletedAt sat
                entity.HasQueryFilter(e => e.DeletedAt == null);
            });
        }
    }
}