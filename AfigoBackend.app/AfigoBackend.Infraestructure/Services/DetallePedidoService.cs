using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Domain.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class DetallePedidoService : IDetallePedidoInterface
    {
        private readonly AppDbContext _db;
        public DetallePedidoService(AppDbContext db) => _db = db;

        public Task<List<DetallePedido>> GetAllAsync()
            => _db.DetallePedidos.AsNoTracking().ToListAsync();

        public Task<DetallePedido?> GetByIdAsync(int id)
            => _db.DetallePedidos.FindAsync(id).AsTask();

        public async Task<DetallePedido> CreateAsync(DetallePedido detallePedido)
        {
            _db.DetallePedidos.Add(detallePedido);
            await _db.SaveChangesAsync();
            return detallePedido;
        }

        public async Task<bool> UpdateAsync(DetallePedido detallePedido)
        {
            if (detallePedido is null || detallePedido.IdDetalle <= 0) { return false; }

            var entity = await _db.DetallePedidos.FindAsync(detallePedido.IdDetalle);
            if (entity is null) return false;

            entity.IdDetalle = detallePedido.IdDetalle;
            entity.PedidoId = detallePedido.PedidoId;
            entity.NombreProducto = detallePedido.NombreProducto;
            entity.CantProducto = detallePedido.CantProducto;
            entity.Descripcion = detallePedido.Descripcion;
            await _db.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.DetallePedidos.FindAsync(id);
            if (entity is null) return false;
            _db.DetallePedidos.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
