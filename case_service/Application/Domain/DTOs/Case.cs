namespace Application.Domain.DTOs;

public class CreateCaseRequest
{
    public string CaseNumber { get; set; }
    public string Title { get; set; }
    public string? ImgPath { get; set; }
}
public class UpdateCaseRequest
{
    public string Title { get; set; }
    public string? ImgPath { get; set; }
}