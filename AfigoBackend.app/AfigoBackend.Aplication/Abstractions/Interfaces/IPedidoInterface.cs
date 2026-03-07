using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Pedido;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IPedidoInterface
    {
        Task<Pedido> CreateAsync(Pedido pedido);
        Task<bool> DeleteAsync(int id);
        Task<List<Pedido>> GetAllAsync();
        Task<List<Pedido>> GetAllByTipoCotizacion();
        Task<List<Pedido>> GetAllByTipoPedido();
        Task<Pedido?> GetByIdAsync(int id);
        Task<List<Pedido>> GetByIdUsuario(int idUsuario);
        Task<List<PedidoConDetalleDto>> GetCotizacionesConDetalles();
        Task<List<PedidoConDetalleDto>> GetPedidosConDetalles();
        Task<bool> UpdateAsync(Pedido pedido);
    }
}
