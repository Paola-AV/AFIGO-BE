using AfigoBackend.Aplication.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VendedorController : ControllerBase
    {
        private readonly IVendedorInterface _service;
        public VendedorController(IVendedorInterface service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllNombres());
    }
}
