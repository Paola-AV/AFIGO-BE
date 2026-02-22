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
        Task<List<VentaDTO>> GetByTrabajadorId(int id);
    }
}
