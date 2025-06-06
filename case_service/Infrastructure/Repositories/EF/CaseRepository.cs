using Application.Domain.Entities;
using Application.Interfaces;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EF
{
    public class CaseRepository : ICaseRepository
    {
        private readonly IDbContext _context;
        
        public CaseRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<Case> CreateCaseAsync(Case newCase)
        {
            _context.Cases.Add(newCase);
            await _context.SaveChangesAsync();
            return newCase;
        }

        public async Task<Case?> GetCaseByIdAsync(int id)
        {
            return await _context.Cases.FindAsync(id);
        }

        public async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            return await _context.Cases.ToListAsync();
        }

        public async Task<bool> UpdateCaseAsync(Case updatedCase)
        {
            var existing = await _context.Cases.FindAsync(updatedCase.Id);
            if (existing == null) return false;

            existing.Title = updatedCase.Title;
            existing.CaseNumber = updatedCase.CaseNumber;
            existing.ImgPath = updatedCase.ImgPath;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCaseAsync(int id)
        {
            var existing = await _context.Cases.FindAsync(id);
            if (existing == null || existing.DeletedAt != null) return false;

            existing.DeletedAt = DateTime.UtcNow;
            existing.IsActive = false;

            _context.Cases.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}