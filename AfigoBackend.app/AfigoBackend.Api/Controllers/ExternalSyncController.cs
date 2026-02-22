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
    }
}
