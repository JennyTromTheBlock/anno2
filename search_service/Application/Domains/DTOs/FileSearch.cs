namespace Application.Domains.DTOs;

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


public class CaseSearchQueryDto
{
    public string Query { get; set; } = string.Empty;
    public int CaseId { get; set; }
    public int DocumentId { get; set; }
    public bool PartialMatch { get; set; } = false;
    public bool? Fuzzy { get; set; } = false;
}
