using AfigoBackend.Domain.Pedido;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Infraestructure.Util;

namespace AfigoBackend.Infraestructure.Services
{
    public class PedidoService : IPedidoInterface
    {
        private readonly AppDbContext _db;
        public PedidoService(AppDbContext db) => _db = db;

        public Task<List<Pedido>> GetAllAsync()
           => _db.Pedidos.AsNoTracking().ToListAsync();

        public Task<Pedido?> GetByIdAsync(int id)
           => _db.Pedidos.FindAsync(id).AsTask();

        public Task<List<Pedido>> GetByIdUsuario(int idUsuario)
           => _db.Pedidos
                .AsNoTracking()
                .Where(p => p.IdUsuario == idUsuario)
                .ToListAsync();

        public Task<List<Pedido>> GetAllByTipoPedido()
           => _db.Pedidos
                .AsNoTracking()
                .Where(p => p.TipoPedido == Constants.TiposDocumento.Pedido)
                .ToListAsync();

        public Task<List<Pedido>> GetAllByTipoCotizacion()
          => _db.Pedidos
               .AsNoTracking()
               .Where(p => p.TipoPedido == Constants.TiposDocumento.Cotizacion)
               .ToListAsync();

        public async Task<bool> UpdateAsync(Pedido pedido)
        {
            if (pedido is null || pedido.IdPedido <= 0) { return false; }
               

            var entity = await _db.Pedidos.FindAsync(pedido.IdPedido);

            if (entity is null) { return false; }

            entity.IdUsuario = pedido.IdUsuario;
            entity.FechaPedido = pedido.FechaPedido;
            entity.Estado = pedido.Estado;
            entity.IdUsuario = pedido.IdUsuario;
            entity.NombreCliente = pedido.NombreCliente;
            entity.FacturaElectronica = pedido.FacturaElectronica;
            entity.DetalleFactura = pedido.DetalleFactura;
            entity.MetodoEnvio = pedido.MetodoEnvio;
            entity.DireccionEnvio = pedido.DireccionEnvio;
            entity.UrgenciaEnvio = pedido.UrgenciaEnvio;
            entity.TipoPedido = pedido.TipoPedido;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Pedido> CreateAsync(Pedido pedido)
        {
            var entity = (await _db.Pedidos.AddAsync(pedido)).Entity;
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Pedidos.FindAsync(id);
            if (entity is null) return false;
            _db.Pedidos.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
