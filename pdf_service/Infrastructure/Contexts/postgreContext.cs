using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Contexts
{
    public class AppDbContext : DbContext, IDbContext
    {
        public DbSet<PdfFile> PdfFiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbConnection GetDbConnection() => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PDF File
            modelBuilder.Entity<PdfFile>(entity =>
            {
                entity.ToTable("pdf_files");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(p => p.Path)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

    
                entity.Property(p => p.DeletedAt); // nullable datetime

                entity.HasQueryFilter(p => p.DeletedAt == null);
            });
        }
    }
}