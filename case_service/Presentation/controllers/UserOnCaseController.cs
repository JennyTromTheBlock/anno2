using Application.Domain.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers;

[ApiController]
[Route("api/cases/{caseId}/users")]
public class UserOnCaseController : ControllerBase
{
    private readonly IUserOnCaseService _service;

    public UserOnCaseController(IUserOnCaseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWithCaseStatusDto>>> GetUsers(int caseId)
    {
        var users = await _service.GetAllUsersWithStatusForCaseAsync(caseId);
        return Ok(users);
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> AddUserToCase(int caseId, int userId, [FromBody] int roleId)
    {
        await _service.AddUserToCaseAsync(userId, caseId, roleId);
        return NoContent();
    }

    [HttpPut("{userId}/role")]
    public async Task<IActionResult> UpdateUserRole(int caseId, int userId, [FromBody] int roleId)
    {
        await _service.UpdateUserRoleOnCaseAsync(userId, caseId, roleId);
        return NoContent();
    }
}