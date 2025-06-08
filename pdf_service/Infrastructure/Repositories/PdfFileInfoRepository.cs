using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

using Infrastructure.Contexts;
using System;
using System.Threading.Tasks;

public class PdfFileInfoRepository : IPdfFileInfoRepository
{
    private readonly AppDbContext _context;

    public PdfFileInfoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PdfFile?> GetByIdAsync(int id)
    {
        return await _context.PdfFiles.FindAsync(id);
        // Respekterer soft delete query filter
    }
    
    public async Task<List<PdfFile>> GetByIdsAsync(List<int> ids)
    {
        return await _context.PdfFiles
            .Where(pdf => ids.Contains(pdf.Id))
            .ToListAsync();
    }
    public async Task<PdfFile> CreateAsync(PdfFile pdfFile)
    {
        await _context.PdfFiles.AddAsync(pdfFile);
        await _context.SaveChangesAsync();
        return pdfFile;
    }

    public async Task UpdateAsync(PdfFile pdfFile)
    {
        _context.PdfFiles.Update(pdfFile);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var pdfFile = await _context.PdfFiles.FindAsync(id);
        if (pdfFile == null) return;

        pdfFile.DeletedAt = DateTime.UtcNow;
        _context.PdfFiles.Update(pdfFile);
        await _context.SaveChangesAsync();
    }
}
