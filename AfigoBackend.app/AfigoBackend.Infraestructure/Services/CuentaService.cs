using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Cuenta;
using AfigoBackend.Domain.Venta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class CuentaService:ICuentaInterface
    {
        private readonly AppDbContext _db;
        public CuentaService(AppDbContext db) => _db = db;

        public Task<List<Cuenta>> GetAllAsync()
          => _db.Cuentas.AsNoTracking().ToListAsync();


        public async Task<List<CuentaDto>> GetCuentasParaExcel()
        {
            var query =
                from c in _db.Cuentas.AsNoTracking()
                join f in _db.Facturas.AsNoTracking() on c.IdFactura equals f.IdFactura into fj
                from f in fj.DefaultIfEmpty()
                join p in _db.Proveedores.AsNoTracking() on c.IdProveedor equals p.IdProveedor into pj
                from p in pj.DefaultIfEmpty() 
                select new CuentaDto
                {
                    Monto = c.Monto,
                    Saldo = c.Saldo,
                    Estado = c.Estado,
                    EstadoFactura = f != null ? f.Estado : null,
                    SucursalFactura = f != null ? f.Sucursal : null,
                    FechaFactura = f != null ? f.Fecha : (DateTime?)null, 
                    NumeroFactura = f != null ? f.Numero : null,
                    ProveedorNombre = p != null ? p.PrimerNombre : null
                };

            return await query.ToListAsync();
        }

    }
}
