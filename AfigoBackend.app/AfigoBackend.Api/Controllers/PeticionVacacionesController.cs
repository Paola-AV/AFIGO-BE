using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.PeticionVacaciones;
using AfigoBackend.Domain.Trabajador;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeticionVacacionesController : ControllerBase
    {

        private readonly IPeticionVacacionesInterface _service;
        public PeticionVacacionesController(IPeticionVacacionesInterface service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("trabajador/{id:int}")]
        public async Task<IActionResult> GetByTrabajadorId(int id)
        {
            var item = await _service.GetByIdTrabajador(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PeticionVacaciones model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.IdTrabajador }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PeticionVacaciones model)
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
