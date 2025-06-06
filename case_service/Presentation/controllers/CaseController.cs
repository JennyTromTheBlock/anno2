using Application.Domain.Entities;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseController : ControllerBase
    {
        private readonly CaseService _service;

        public CaseController(CaseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Case newCase)
        {
            var created = await _service.CreateCaseAsync(newCase);
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
        public async Task<IActionResult> Update(int id, Case updateCase)
        {
            if (id != updateCase.Id) return BadRequest("ID mismatch");
            var success = await _service.UpdateCaseAsync(updateCase);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteCaseAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}