namespace Application.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<UserOnCase> UserRoles { get; set; } = new List<UserOnCase>();
}