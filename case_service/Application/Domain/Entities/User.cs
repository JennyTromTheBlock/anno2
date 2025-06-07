namespace Application.Domain.Entities;


public class User
{
    public int Id { get; set; }
    public string Initials { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? DeletedAt { get; set; }

    public ICollection<UserOnCase> Cases { get; set; } = new List<UserOnCase>();
}