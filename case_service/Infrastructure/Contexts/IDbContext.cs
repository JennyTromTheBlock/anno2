using System.Data.Common;
using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

    public interface IDbContext
    {
        DbSet<Case> Cases { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbConnection GetDbConnection();
    }

