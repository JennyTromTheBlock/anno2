using Application.Domain.DTOs;
using Application.Domain.Entities;
using Application.Interfaces;
using Application.Services.Interfaces;
namespace Application.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _repo;

        public CaseService(ICaseRepository repo)
        {
            _repo = repo;
        }
        
        public async Task<Case> CreateCaseAsync(CreateCaseRequest request, int userId)
        {
            var newCase = new Case
            {
                CaseNumber = request.CaseNumber,
                Title = request.Title,
                AuthorId = userId,
                ImgPath = request.ImgPath,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                DeletedAt = null
            };

            return await _repo.CreateCaseAsync(newCase);
        }

        public Task<Case?> GetCaseByIdAsync(int id) => _repo.GetCaseByIdAsync(id);

        public Task<IEnumerable<Case>> GetAllCasesAsync() => _repo.GetAllCasesAsync();

        public async Task<bool> UpdateCaseAsync(int id, UpdateCaseRequest updateRequest)
        {
            var existingCase = await _repo.GetCaseByIdAsync(id);
            if (existingCase == null || existingCase.DeletedAt != null)
                return false;

            existingCase.Title = updateRequest.Title;
            existingCase.ImgPath = updateRequest.ImgPath;
            existingCase.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateCaseAsync(existingCase);
        }
        public Task<bool> DeleteCaseAsync(int id) => _repo.DeleteCaseAsync(id);
    }
}


