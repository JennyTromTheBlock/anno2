using Application.Domain.DTOs;
using Application.Domain.Entities;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilter;


namespace Presentation.Controllers;

[AuthActionFilter]
[ApiController]
[Route("api/[controller]")]
public class CaseController : ControllerBase
{
    private readonly ICaseService _service;

    public CaseController(ICaseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCaseRequest newCase)
    {
        var user = HttpContext.Items["User"] as UserRequest;

        var created = await _service.CreateCaseAsync(newCase, user.UserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var found = await _service.GetCaseByIdAsync(id);
        return found == null ? NotFound() : Ok(found);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cases = await _service.GetAllCasesAsync();
        return Ok(cases);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCaseRequest updateRequest)
    {
        var success = await _service.UpdateCaseAsync(id, updateRequest);
        return success ? NoContent() : NotFound();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteCaseAsync(id);
        if (!success) return NotFound();

        return Ok(new { DeletedId = id });
    }
}
