using Application.Domain.DTOs;
using Application.Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface ICaseService
    {
        Task<Case> CreateCaseAsync(CreateCaseRequest request, int userId);
        Task<Case?> GetCaseByIdAsync(int id);
        Task<IEnumerable<Case>> GetAllCasesAsync();
        Task<bool> UpdateCaseAsync(int id, UpdateCaseRequest updateRequest);
        Task<bool> DeleteCaseAsync(int id);
    }
}
