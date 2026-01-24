
using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioInterface _service;

        public UsuariosController(IUsuarioInterface service)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Usuario model)
        {
            var ok = await _service.UpdateAsync( model);
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
