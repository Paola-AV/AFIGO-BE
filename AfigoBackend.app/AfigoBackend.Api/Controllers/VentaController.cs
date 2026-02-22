using AfigoBackend.Aplication.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaInterface _service;
        public VentaController(IVentaInterface service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllByTrabajador(int id)
        {
            var ventas = await _service.GetByTrabajadorId(id);
            if (ventas == null || ventas.Count == 0) return NotFound();
            return Ok(ventas);
        }
    }
}
