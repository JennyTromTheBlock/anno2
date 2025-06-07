using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Contexts
{
    public class AppDbContext : DbContext, IDbContext
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<Attachment> Attachments { get; set; }  // Tilføjet DbSet for Attachment

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
               entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
       
               entity.Property(e => e.IsActive).HasDefaultValue(true);
       
               entity.HasQueryFilter(e => e.DeletedAt == null);
           });
       
           modelBuilder.Entity<Attachment>(entity =>
           {
               entity.ToTable("attachment");
               entity.HasKey(e => e.Id);
       
               entity.Property(e => e.Title).IsRequired();
               entity.Property(e => e.Position).HasDefaultValue(0);
       
               entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
               entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
       
               // Ingen cascade delete - blot soft delete
               entity.HasOne<Case>()
                     .WithMany()
                     .HasForeignKey(a => a.CaseId)
                     .OnDelete(DeleteBehavior.Restrict);
       
               entity.HasQueryFilter(a => a.DeletedAt == null);
           });
       }

    }
}

