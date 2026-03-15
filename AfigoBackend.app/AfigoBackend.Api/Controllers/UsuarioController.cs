
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

        [HttpGet("usuariotrabajador")]
        public async Task<IActionResult> GetAllUsuarioTrabajador() => Ok(await _service.GetAllUsuarioTrabajadorAsync());

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
        public async Task<IActionResult> Update([FromBody] UsuarioTrabajadorUpdate usuario)
        {
            var correo = usuario.Correo?.Trim().ToLowerInvariant() ?? string.Empty;
            var nombreUsuario = usuario.NombreDeUsuario?.Trim().ToLowerInvariant() ?? string.Empty;
            var vendedor = usuario.Vendedor ?? false;
            var trabajadorId = usuario.TrabajadorId ?? null;
            var ok = await _service.UpdateAsync( 
                usuario.UserId,
                usuario.TrabajadorId,
                correo,
                nombreUsuario,
                usuario.Nombre,
                usuario.NombreVendedor,
                usuario.UsuarioAdmin ? 1 : 0,
                vendedor ? 1 : 0
                );
            return ok ? NoContent() : NotFound();
        }

        [HttpPut("inactivo/{id:int}")]
        public async Task<IActionResult> SetInactivo( int id)
        {
            var ok = await _service.InactivarUsuario(id);
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
public record UsuarioTrabajadorUpdate(
    int UserId,
    int? TrabajadorId,
    string Correo,
    string Nombre,
    string NombreDeUsuario,
    string? NombreVendedor,
    bool UsuarioAdmin,
    bool? Vendedor
);