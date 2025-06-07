using Application.Domain.DTOs;
using Application.Domain.Entities;
using Application.Services.Interfaces;
using Application.Interfaces;


namespace Application.Services;

public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _repo;

        public AttachmentService(IAttachmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Attachment> CreateAsync(AttachmentCreateRequest request, int userId)
        {
            var attachment = new Attachment
            {
                CaseId = request.CaseId,
                Title = request.Title,
                Author = userId,
                LastEditBy = userId,
                Position = 0, // default value, kan evt. tilføjes i request senere
                Description = request.Description,
                ImgPath = request.ImgPath,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            return await _repo.CreateAsync(attachment);
        }


        public async Task<Attachment?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Attachment>> GetAllByCaseIdAsync(int caseId)
        {
            return await _repo.GetByCaseIdAsync(caseId);
        }

        public async Task<bool> UpdateAsync(int id, AttachmentUpdateRequest request, int userId)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null || existing.DeletedAt != null)
                return false;

            // Map update felter
            existing.Title = request.Title ?? existing.Title;
            existing.Description = request.Description ?? existing.Description;
            existing.ImgPath = request.ImgPath ?? existing.ImgPath;

            existing.LastEditBy = userId;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }