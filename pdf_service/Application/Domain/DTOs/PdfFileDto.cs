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
    public int AttId { get; set; } = 0;
    public IFormFile File { get; set; } = default!;
}


public class PdfWordEntry
{
    public string DocumentId { get; set; }
    public int Page { get; set; }
    public string Word { get; set; }
    public Position Position { get; set; }
}

public class Position
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}

