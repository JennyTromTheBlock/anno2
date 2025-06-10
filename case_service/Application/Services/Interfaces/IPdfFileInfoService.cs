namespace Application.Services.Interfaces;

// IPdfFileInfoService.cs
using Application.Domain.Entities;

public interface IPdfFileInfoService
{
    Task<PdfFileInfo?> GetByIdAsync(int id);
    Task CreateAsync(PdfFileInfo pdf);
    Task DeleteAsync(int id);
}
