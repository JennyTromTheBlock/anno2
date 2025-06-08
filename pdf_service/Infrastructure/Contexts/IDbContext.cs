using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public interface IDbContext
{
    DbSet<PdfFile> PdfFiles { get; }

    DbConnection GetDbConnection();
}
