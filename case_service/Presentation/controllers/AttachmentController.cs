using Application.Domain.Entities;
using Application.Domain.DTOs;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Presentation.ActionFilter;


namespace Presentation.Controllers;

[AuthActionFilter]
[ApiController]
[Route("api/[controller]")]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _service;

    public AttachmentController(IAttachmentService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttachmentCreateRequest request)
    {
        var user = HttpContext.Items["User"] as User;

        var created = await _service.CreateAsync(request, user.UserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var attachment = await _service.GetByIdAsync(id);
        if (attachment == null) return NotFound();
        return Ok(attachment);
    }

    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetAllByCaseId(int caseId)
    {
        var attachments = await _service.GetAllByCaseIdAsync(caseId);
        return Ok(attachments);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AttachmentUpdateRequest request)
    {
        var user = HttpContext.Items["User"] as User;
    
        var updated = await _service.UpdateAsync(id, request, user.UserId);
        if (!updated) return NotFound();
    
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
    
        return Ok(new { DeletedId = id });
    }
}
