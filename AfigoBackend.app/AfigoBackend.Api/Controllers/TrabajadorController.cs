using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Trabajador;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrabajadorController : ControllerBase
    {
        private readonly ITrabajadorInterface _service;
        public TrabajadorController(ITrabajadorInterface service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("user/{id:int}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var item = await _service.GetByUsuarioIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Trabajador model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.IdTrabajador }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Trabajador model)
        {
            var ok = await _service.UpdateAsync(model);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
