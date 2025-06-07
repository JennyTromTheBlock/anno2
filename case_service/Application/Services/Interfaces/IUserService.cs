using Application.Domain.DTOs;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> GetByIdAsync(int id);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task CreateAsync(CreateUserRequest request);
    Task UpdateAsync(int id, UpdateUserRequest request);
    Task SoftDeleteAsync(int id);
}
