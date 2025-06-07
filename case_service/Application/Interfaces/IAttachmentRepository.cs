using Application.Domain.Entities;
namespace Application.Interfaces;

public interface IAttachmentRepository
{
    Task<Attachment?> GetByIdAsync(int id);
    Task<IEnumerable<Attachment>> GetByCaseIdAsync(int caseId);
    Task<Attachment> CreateAsync(Attachment attachment);
    Task<bool> UpdateAsync(Attachment attachment);
    Task<bool> DeleteAsync(int id);
}

