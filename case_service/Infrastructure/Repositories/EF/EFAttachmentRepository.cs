using Application.Domain.Entities;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Infrastructure.Repositories.EF;


public class AttachmentRepository : IAttachmentRepository
{
    private readonly IDbContext _context;

    public AttachmentRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<Attachment?> GetByIdAsync(int id)
    {
        return await _context.Attachments
            .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null);
    }

    public async Task<IEnumerable<Attachment>> GetByCaseIdAsync(int caseId)
    {
        return await _context.Attachments
            .AsNoTracking()
            .Where(a => a.CaseId == caseId && a.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<Attachment> CreateAsync(Attachment attachment)
    {
        // Find højeste position for attachments på samme case, ekskluder slettede
        var maxPosition = await GetMaxPositionForCaseAsync(attachment.CaseId);
    
        // Sæt position til max + 1
        attachment.Position = maxPosition + 1;
    
        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();
        return attachment;
    }

    public async Task<bool> UpdateAsync(Attachment attachment)
    {
        _context.Attachments.Update(attachment);
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Attachments.FindAsync(id);
        if (existing == null || existing.DeletedAt != null)
            return false;

        existing.DeletedAt = DateTime.UtcNow;
        _context.Attachments.Update(existing);
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }
    
    private async Task<int> GetMaxPositionForCaseAsync(int caseId)
    {
        return await _context.Attachments
            .Where(a => a.CaseId == caseId && a.DeletedAt == null)
            .MaxAsync(a => (int?)a.Position) ?? 0;
    }
}
