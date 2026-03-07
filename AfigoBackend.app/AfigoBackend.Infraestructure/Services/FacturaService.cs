using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Domain.Factura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class FacturaService:IFacturaInterface
    {
        private readonly AppDbContext _db;
        public FacturaService(AppDbContext db) => _db = db;

        public Task<List<Factura>> GetAllAsync()
          => _db.Facturas.AsNoTracking().ToListAsync();

        public async Task<List<FacturaDto>> GetFacturasParaExcel()
        {
            var query =
                from f in _db.Facturas.AsNoTracking()
                join p in _db.Proveedores.AsNoTracking() on f.IdProveedor equals p.IdProveedor into pj
                from p in pj.DefaultIfEmpty()
                select new FacturaDto
                {
                    FechaFactura = f.Fecha,
                    Numero = f.Numero,
                    Estado = f.Estado,
                    Sucursal = f.Sucursal,
                    ProveedorNombre = p != null ? p.PrimerNombre : null
                };

            return await query.ToListAsync();
        }
    }
}
