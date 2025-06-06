using Application.Domain.DTOs;
using Application.Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface ICaseService
    {
        Task<Case> CreateCaseAsync(CreateCaseRequest request);
        Task<Case?> GetCaseByIdAsync(int id);
    }
}