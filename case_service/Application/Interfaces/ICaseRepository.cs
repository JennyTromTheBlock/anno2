using Application.Domain.Entities;

namespace Application.Interfaces;

public interface ICaseRepository
{
    Task<Case> CreateCaseAsync(Case newCase);
    Task<Case?> GetCaseByIdAsync(int id);
    Task<IEnumerable<Case>> GetAllCasesAsync();
    Task<bool> UpdateCaseAsync(Case updatedCase);
    Task<bool> DeleteCaseAsync(int id);
}