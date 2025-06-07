
namespace Infrastructure.Contexts;
public interface IPdfService
{
    Task<IEnumerable<PdfFileDto>> GetAllAsync();
    Task<PdfFileDto?> GetByIdAsync(int id);
    Task UploadAsync(byte[] content, string fileName);
}