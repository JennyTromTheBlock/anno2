namespace Application.Interfaces;

// IPdfFileInfoRepository.cs
using Application.Domain.Entities;

public interface IPdfFileInfoRepository
{
    Task<PdfFileInfo?> GetByIdAsync(int id);
    Task CreateAsync(PdfFileInfo pdf);
    Task DeleteAsync(int id);
}
