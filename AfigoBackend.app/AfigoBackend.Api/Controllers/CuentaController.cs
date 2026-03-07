using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Infraestructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaInterface _service;
        private readonly IExcelExporter _excel;
        public CuentaController(ICuentaInterface service, IExcelExporter excel)
        {
            _service = service;
            _excel = excel;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());


        [HttpGet("excel")]
        public async Task<IActionResult> Export()
        {
            var cuentas = await _service.GetCuentasParaExcel();

            var bytes = _excel.Create(cuentas, sheetName: "Cuentas");

            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Cuentas_{DateTime.UtcNow:yyyy-MM-dd}.xlsx";
            return File(bytes, contentType, fileName);
        }

    }
}
