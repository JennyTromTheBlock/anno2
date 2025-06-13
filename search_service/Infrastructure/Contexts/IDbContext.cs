using System.Data.Common;
using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

    public interface IDbContext
    {
        DbSet<Case> Cases { get; }
        DbSet<Attachment> Attachments { get; }
        DbSet<User> Users { get; }
        DbSet<UserOnCase> UsersOnCase { get; }
        
        DbSet<PdfFileInfo> PdfFileInfos { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbConnection GetDbConnection();
    }

