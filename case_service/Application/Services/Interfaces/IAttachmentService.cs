using Application.Domain.Entities;
using Application.Domain.DTOs;

namespace Application.Services.Interfaces;

public interface IAttachmentService
{
    Task<Attachment> CreateAsync(AttachmentCreateRequest request, int userId);
    Task<Attachment?> GetByIdAsync(int id);
    Task<IEnumerable<Attachment>> GetAllByCaseIdAsync(int caseId);
    Task<bool> UpdateAsync(int id, AttachmentUpdateRequest request, int userId);
    Task<bool> DeleteAsync(int id);
}

