using AfigoBackend.Domain.VentaDetalle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IVentaDetalleInterface
    {
        Task<List<VentaDetalle>> GetAllAsync();
        Task<VentaDetalle?> GetByIdAsync(int id);
    }
}
