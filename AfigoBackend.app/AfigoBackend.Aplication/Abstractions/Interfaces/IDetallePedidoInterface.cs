using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Domain.DetallePedido;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IDetallePedidoInterface
    {
        Task<DetallePedido> CreateAsync(DetallePedido detallePedido);
        Task<bool> DeleteAsync(int id);
        Task<List<DetallePedido>> GetAllAsync();
        Task<DetallePedido?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(DetallePedido detallePedido);
    }
}
