using Application.Domain.Entities;
using Application.Domain.DTOs;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;


namespace Infrastructure.Repositories.EF;

public class EFUserOnCaseRepository : IUserOnCaseRepository
{
    private readonly IDbContext _context;

    public EFUserOnCaseRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserWithCaseStatusDto>> GetAllUsersWithStatusForCaseAsync(int caseId)
    {
        var allUsers = await _context.Users
            .Select(user => new UserWithCaseStatusDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Initials = user.Initials,
                Role = _context.UsersOnCase
                        .Where(uoc => uoc.CaseId == caseId && uoc.UserId == user.Id)
                        .Select(uoc => uoc.Role != null ? uoc.Role.Name : null)
                        .FirstOrDefault()
            })
            .ToListAsync();

        return allUsers;
    }

    public async Task AddUserToCaseAsync(int userId, int caseId, int roleId)
    {
        var entry = await _context.UsersOnCase
            .FirstOrDefaultAsync(u => u.UserId == userId && u.CaseId == caseId);

        if (entry == null)
        {
            var newUserOnCase = new UserOnCase
            {
                UserId = userId,
                CaseId = caseId,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UsersOnCase.Add(newUserOnCase);
            await _context.SaveChangesAsync();
        }
        // Optional: throw if already exists
    }

    public async Task UpdateUserRoleOnCaseAsync(int userId, int caseId, int roleId)
    {
        var entry = await _context.UsersOnCase
            .FirstOrDefaultAsync(u => u.UserId == userId && u.CaseId == caseId);

        if (entry != null)
        {
            entry.RoleId = roleId;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("User is not assigned to the case.");
        }
    }
}
