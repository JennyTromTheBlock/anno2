namespace Application.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task SoftDeleteAsync(int id);
}