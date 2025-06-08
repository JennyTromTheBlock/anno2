
namespace Application.Interfaces.Services;

public interface IPdfFileInfoService
{
    Task<PdfFile?> GetByIdAsync(int id);
    Task<List<PdfFile>> GetByIdsAsync(List<int> ids);

    Task<PdfFile> CreateAsync(PdfFileCreateDto pdfFile, int userId, string path);
    Task UpdateAsync(PdfFile pdfFile);
    Task SoftDeleteAsync(int id);
}