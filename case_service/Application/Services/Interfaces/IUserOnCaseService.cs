using Application.Domain.DTOs;

namespace Application.Services.Interfaces;

public interface IUserOnCaseService
{
    Task<IEnumerable<UserWithCaseStatusDto>> GetAllUsersWithStatusForCaseAsync(int caseId);
    Task AddUserToCaseAsync(int userId, int caseId, int roleId);
    Task UpdateUserRoleOnCaseAsync(int userId, int caseId, int roleId);
}