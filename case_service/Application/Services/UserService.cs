using Application.Domain.DTOs;
using Application.Domain.Entities;
using Application.Services.Interfaces;
using Application.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserResponse> GetByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Initials = user.Initials
        };
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Initials = user.Initials
        });
    }

    public async Task CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Initials = request.Initials,
            DeletedAt = null
        };

        await _repo.AddAsync(user);
    }

    public async Task UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");

        user.Name = request.Name;
        user.Email = request.Email;
        user.Initials = request.Initials;

        await _repo.UpdateAsync(user);
    }

    public async Task SoftDeleteAsync(int id)
    {
        await _repo.SoftDeleteAsync(id);
    }
}
