using AfigoBackend.Domain.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.DTO;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IVentaInterface
    {
        Task<List<Venta>> GetAllAsync();
        Task<Dictionary<string, double>> GetAllComisionMensualPorVendedorAsync();
        Task<List<VentaDTO>> GetByTrabajadorId(int id);
        Task<double> GetComisionMensualPorVendedorAsync(string nombreVendedor);
        Task<List<VentaGetDto>> GetVentasConDetallesAsync( DateTime desde, DateTime hasta);
        Task<List<VentaGetDto>> GetVentasConDetallesPorVendedorAsync(DateTime desde, DateTime hasta, string nombreVendedor);
        Task<List<VentasDetallesExcelDto>> GetVentasParaExcelAsync(DateTime desde, DateTime hasta);
    }
}
