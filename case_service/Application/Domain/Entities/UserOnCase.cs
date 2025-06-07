namespace Application.Domain.Entities;

public class UserOnCase
{
    public int UserId { get; set; }
    public int CaseId { get; set; }
    public int? RoleId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Role? Role { get; set; }
}