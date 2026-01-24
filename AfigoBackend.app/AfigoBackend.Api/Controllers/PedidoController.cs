using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Pedido;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoInterface _service;
        public PedidoController(IPedidoInterface service)
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
            var items = await _service.GetByIdUsuario(id);
            return Ok(items);
        }

        [HttpGet("pedido")]
        public async Task<IActionResult> GetByTipoPedido()
        {
            var items = await _service.GetAllByTipoPedido();
            return Ok(items);
        }

        [HttpGet("cotizacion")]
        public async Task<IActionResult> GetByTipoCotizacion()
        {
            var items = await _service.GetAllByTipoCotizacion();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pedido model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetByUserId), new { id = created.IdUsuario }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Pedido model)
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
