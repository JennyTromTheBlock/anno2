using System.Data.Common;
using Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public interface IDbContext
{
    DbSet<PdfFile> PdfFiles { get; }

    DbConnection GetDbConnection();
}
