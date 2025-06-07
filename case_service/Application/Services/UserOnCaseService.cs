using Application.Interfaces;
using Application.Services.Interfaces;
using Application.Domain.DTOs;

namespace Application.Services;

public class UserOnCaseService : IUserOnCaseService
{
    private readonly IUserOnCaseRepository _repository;

    public UserOnCaseService(IUserOnCaseRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<UserWithCaseStatusDto>> GetAllUsersWithStatusForCaseAsync(int caseId)
    {
        return _repository.GetAllUsersWithStatusForCaseAsync(caseId);
    }

    public Task AddUserToCaseAsync(int userId, int caseId, int roleId)
    {
        return _repository.AddUserToCaseAsync(userId, caseId, roleId);
    }

    public Task UpdateUserRoleOnCaseAsync(int userId, int caseId, int roleId)
    {
        return _repository.UpdateUserRoleOnCaseAsync(userId, caseId, roleId);
    }
}