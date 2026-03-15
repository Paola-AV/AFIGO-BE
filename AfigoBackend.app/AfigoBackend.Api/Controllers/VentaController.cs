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

        [HttpGet("detalles")]
        public async Task<IActionResult> GetConDetalles([FromQuery] DateTime desde,[FromQuery] DateTime hasta) => 
            Ok(await _service.GetVentasConDetallesAsync(desde, hasta));

        [HttpGet("detalles/vendedor")]
        public async Task<IActionResult> GetConDetallesPorVendedor([FromQuery] DateTime desde,[FromQuery] DateTime hasta,[FromQuery] string nombreVendedor)
        {
            if (string.IsNullOrWhiteSpace(nombreVendedor))
                return BadRequest(new { error = "El nombre del vendedor es obligatorio." });

            return Ok(await _service.GetVentasConDetallesPorVendedorAsync(desde, hasta, nombreVendedor));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllByTrabajador(int id)
        {
            var ventas = await _service.GetByTrabajadorId(id);
            if (ventas == null || ventas.Count == 0) return NotFound();
            return Ok(ventas);
        }

        [HttpGet("comision")]
        public async Task<IActionResult> GetComision([FromQuery] string nombreVendedor)  // default 5%
        {
            if (string.IsNullOrWhiteSpace(nombreVendedor))
                return BadRequest(new { error = "El nombre del vendedor es obligatorio." });

            var comision = await _service.GetComisionMensualPorVendedorAsync(nombreVendedor );

            return Ok(new
            {
                nombreVendedor,
                mes = DateTime.UtcNow.ToString("MMMM yyyy"),
                porcentajeComision = 1.3,
                comision
            });
        }

        [HttpGet("comision/todas")]
        public async Task<IActionResult> GetTodasComisiones()
        {
            var comisiones = await _service.GetAllComisionMensualPorVendedorAsync();
            return Ok(new
            {
                mes = DateTime.UtcNow.ToString("MMMM yyyy"),
                porcentajeComision = 1.3,
                comisiones
            });
        }
    }
}
