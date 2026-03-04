using AfigoBackend.Aplication.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalSyncController : ControllerBase
    {
        private readonly IExternalSyncInterface _syncService;
        public ExternalSyncController(IExternalSyncInterface syncService) => _syncService = syncService;

        [HttpPost("sync")]
        public async Task<IActionResult> SyncAll()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncAllAsync(ct);
            return Accepted();
        }

        [HttpPost("syncCuentas")]
        public async Task<IActionResult> SyncCuentas()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncCuentas(ct);
            return Accepted();
        }

        [HttpPost("syncVentas")]
        public async Task<IActionResult> SyncVentas()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncVentas(ct);
            return Accepted();
        }

        [HttpPost("syncFacturas")]
        public async Task<IActionResult> SyncFacturas()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncFacturas(ct);
            return Accepted();
        }

        [HttpPost("syncInventario")]
        public async Task<IActionResult> SyncInventario()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncInventario(ct);
            return Accepted();
        }

        [HttpPost("syncGasto")]
        public async Task<IActionResult> SyncGasto()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncGasto(ct);
            return Accepted();
        }

        [HttpPost("syncProveedor")]
        public async Task<IActionResult> SyncProveedor()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncProveedores(ct);
            return Accepted();
        }

        [HttpPost("syncProducto")]
        public async Task<IActionResult> SyncProducto()
        {
            var ct = HttpContext.RequestAborted;
            await _syncService.SyncProductos(ct);
            return Accepted();
        }
    }
}
