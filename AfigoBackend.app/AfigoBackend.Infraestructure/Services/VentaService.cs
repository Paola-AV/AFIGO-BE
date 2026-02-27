using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Factura;
using AfigoBackend.Domain.Usuario;
using AfigoBackend.Domain.Venta;
using AfigoBackend.Domain.VentaDetalle;
using AfigoBackend.Aplication.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class VentaService:IVentaInterface
    {
        private readonly AppDbContext _db;
        public VentaService(AppDbContext db) => _db = db;

        public Task<List<Venta>> GetAllAsync()
          => _db.Ventas.AsNoTracking().ToListAsync();

        public Task<List<VentaDTO>> GetByTrabajadorId(int id)
        {
            return _db.Ventas
                .AsNoTracking()
                .Where(v => v.IdTrabajador == id)
                .Select(v => new VentaDTO
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Descripcion = v.Descripcion,
                    IdTrabajador = v.IdTrabajador,
                    IdCliente = v.IdCliente,
                    numFactura = v.numFactura,
                    Estado = v.Estado,
                    MontoTotal = v.MontoTotal,
                    Referencia = v.Referencia,
                    VentaDetalles = _db.VentaDetalles
                                    .Where(d => d.IdVenta == v.IdVenta)
                                    .Select(d => new VentaDetalle
                                    {
                                        IdVentaDetalle = d.IdVentaDetalle,
                                        IdVenta = d.IdVenta,
                                        IdProducto = d.IdProducto,
                                        Cantidad = d.Cantidad
                                    })
                                    .ToList()
                })
                .ToListAsync();
        }
    }
}
