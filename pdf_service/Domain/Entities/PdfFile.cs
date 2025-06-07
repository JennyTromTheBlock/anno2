namespace Infrastructure.Contexts;

public class PdfFile
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}