using Application.Interfaces;

namespace Infrastructure.Repositories.EF;

// PdfFileInfoRepository.cs
using Application.Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

public class PdfFileInfoRepository : IPdfFileInfoRepository
{
    private readonly AppDbContext _context;

    public PdfFileInfoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PdfFileInfo?> GetByIdAsync(int id)
    {
        return await _context.PdfFileInfos
            .Include(p => p.Attachment)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(PdfFileInfo pdf)
    {
        _context.PdfFileInfos.Add(pdf);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.PdfFileInfos.FindAsync(id);
        if (entity != null)
        {
            _context.PdfFileInfos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
