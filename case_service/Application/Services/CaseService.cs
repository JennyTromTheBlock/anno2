using Application.Domain.Entities;
using Application.Interfaces;

namespace Application.Services
{
    public class CaseService
    {
        private readonly ICaseRepository _repo;

        public CaseService(ICaseRepository repo)
        {
            _repo = repo;
        }

        public Task<Case> CreateCaseAsync(Case c) => _repo.CreateCaseAsync(c);
        
        public Task<Case?> GetCaseByIdAsync(int id) => _repo.GetCaseByIdAsync(id);
        public Task<IEnumerable<Case>> GetAllCasesAsync() => _repo.GetAllCasesAsync();
        public Task<bool> UpdateCaseAsync(Case c) => _repo.UpdateCaseAsync(c);
        public Task<bool> DeleteCaseAsync(int id) => _repo.DeleteCaseAsync(id);
    }
}


