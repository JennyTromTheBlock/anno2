namespace Application.Interfaces.Repositories;

using System.Threading.Tasks;

public interface IPdfFileInfoRepository
{
    Task<PdfFile?> GetByIdAsync(int id);
    
    Task<List<PdfFile>> GetByIdsAsync(List<int> ids);

    Task<PdfFile> CreateAsync(PdfFile pdfFile);
    Task UpdateAsync(PdfFile pdfFile);
    Task SoftDeleteAsync(int id);
}
