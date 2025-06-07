using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Infrastructure.Contexts
{
    public class AppDbContext : DbContext, IDbContext
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }

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
           
              // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
            
                entity.HasKey(u => u.Id);
            
                entity.Property(u => u.Initials).IsRequired().HasMaxLength(10);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            
                entity.Property(u => u.DeletedAt);  // Nullable datetime
            
                entity.HasQueryFilter(u => u.DeletedAt == null);  // Soft delete filter
            });
           
               // Role
               modelBuilder.Entity<Role>(entity =>
               {
                   entity.ToTable("roles");
           
                   entity.HasKey(r => r.Id);
           
                   entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
               });
           
           // UserOnCase
            modelBuilder.Entity<UserOnCase>(entity =>
            {
                entity.ToTable("users_on_case");
            
                entity.HasKey(uoc => new { uoc.UserId, uoc.CaseId });
            
                entity.Property(uoc => uoc.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
                entity.HasOne(uoc => uoc.User)
                      .WithMany(u => u.Cases)
                      .HasForeignKey(uoc => uoc.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            
                entity.HasOne(uoc => uoc.Case)
                      .WithMany(c => c.Users)
                      .HasForeignKey(uoc => uoc.CaseId)
                      .OnDelete(DeleteBehavior.Cascade); // eller Restrict afhængigt af din strategi
            
                entity.HasOne(uoc => uoc.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(uoc => uoc.RoleId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
           
           modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "read" },
               new Role { Id = 2, Name = "write" },
               new Role { Id = 3, Name = "inactive" }
           );
       }

    }
}

