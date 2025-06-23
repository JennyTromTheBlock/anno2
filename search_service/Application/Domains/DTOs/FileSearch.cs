namespace Application.Domains.DTOs;


public class PdfWord
{
    public string Word { get; set; }
    public Position Position { get; set; }
}

public class SentenceEntry
{
    public string DocumentId { get; set; }
    public int Page { get; set; }
    public string Sentence { get; set; }
    public List<PdfWord> Words { get; set; }
    
    public string? CaseId { get; set; }
    public string? AttachmentId { get; set; }
    public string? FileName { get; set; }
}

public class Position
{
    public float X1 { get; set; }
    public float X2 { get; set; }
    public float Y1 { get; set; }
    public float Y2 { get; set; }
}


public class PdfWithWordsReturnDto
{
    public string DocumentId { get; set; }
    public string Sentence { get; set; }
    public int Page { get; set; }
    public List<PdfWord> Words { get; set; }
}



// bruges til søgning
public class CaseSearchQueryDto
{
    public string? Query { get; set; }
    public string? DocumentId { get; set; }
    public string CaseId { get; set; }                     
    public string? AttachmentId { get; set; }           
    public bool PartialMatch { get; set; } = false;
    public bool? Fuzzy { get; set; } = false;
    public int batchSize { get; set; } = 1000;
}


