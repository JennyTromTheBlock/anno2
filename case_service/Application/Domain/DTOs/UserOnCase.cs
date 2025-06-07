namespace Application.Domain.DTOs;

public class UserWithCaseStatusDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Initials { get; set; } = null!;
    public string? Role { get; set; } // null hvis ikke på sagen
}