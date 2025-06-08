using Microsoft.AspNetCore.Http;

public class PdfFileDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PdfFileCreateDto
{
    public string FileName { get; set; } = string.Empty;
    public IFormFile File { get; set; } = default!;
}