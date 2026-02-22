using AfigoBackend.Domain.Gasto;
using AfigoBackend.Domain.Inventario;
using AfigoBackend.Domain.Producto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.Abstractions.Interfaces;

namespace AfigoBackend.Infraestructure.Services
{
    public class ExternalSyncService:IExternalSyncInterface
    {
        private readonly ExternalDbContext _ext;
        private readonly AppDbContext _app;

        public ExternalSyncService(ExternalDbContext ext, AppDbContext app)
        {
            _ext = ext;
            _app = app;
        }

        public async Task SyncProductosAsync(CancellationToken ct = default)
        {
            var external = await _ext.Productos.AsNoTracking().ToListAsync(ct);

            foreach (var p in external)
            {
                var existing = await _app.Productos
                    .FirstOrDefaultAsync(x => x.IdentificadorExt == p.IdProducto , ct);

                if (existing == null)
                {
                    var nuevo = new Producto
                    {
                        Nombre = p.Nombre ?? string.Empty,
                        Descripcion = p.Descripcion ?? string.Empty,
                        PrecioCosto = Convert.ToDecimal(p.PrecioCosto ?? 0.0),
                        PrecioVenta = Convert.ToDecimal(p.PrecioVenta ?? 0.0),
                        Familia = p.Familia ?? string.Empty,
                        Marca = p.Marca ?? string.Empty,
                        IdentificadorExt = p.IdProducto ?? string.Empty
                    };
                    _app.Productos.Add(nuevo);
                }
                else
                {
                    // Actualiza campos si cambian
                    existing.Nombre = p.Nombre ?? existing.Nombre;
                    existing.Descripcion = p.Descripcion ?? existing.Descripcion;
                    existing.PrecioCosto = Convert.ToDecimal(p.PrecioCosto ?? (double)existing.PrecioCosto);
                    existing.PrecioVenta = Convert.ToDecimal(p.PrecioVenta ?? (double)existing.PrecioVenta);
                    existing.Familia = p.Familia ?? existing.Familia;
                    existing.Marca = p.Marca ?? existing.Marca;
                }
            }

            await _app.SaveChangesAsync(ct);
        }

        public async Task SyncGastosAsync(CancellationToken ct = default)
        {
            var external = await _ext.Gastos.AsNoTracking().ToListAsync(ct);

            foreach (var g in external)
            {
                var fechaGasto = DateOnly.FromDateTime(g.Fecha);

                var existing = await _app.Gastos.FirstOrDefaultAsync(x => x.Tipo == g.Tipo && x.Fecha == fechaGasto, ct);
                
                if ( existing==null)
                {
                    var gasto = new Gasto
                    {
                        Tipo = g.Tipo,
                        Descripcion = g.Descripcion,
                        Monto = Convert.ToDecimal(g.Monto),
                        Fecha = fechaGasto,
                        Sucursal = g.Sucursal
                    };
                    _app.Gastos.Add(gasto);
                }
            }

            await _app.SaveChangesAsync(ct);
        }

        public async Task SyncInventarioAsync(CancellationToken ct = default)
        {
            
            var external = await _ext.Inventarios.AsNoTracking().ToListAsync(ct);

            foreach (var i in external)
            {
                // Buscar producto por Codigo (id_producto externo)
                var product = await _app.Productos.FirstOrDefaultAsync(p => p.IdentificadorExt == i.IdProducto, ct);
                if (product == null)
                {
                   
                    continue;
                }

                var fechaIngreso = DateOnly.FromDateTime(i.FechaIngreso);

                var existing = await _app.Inventarios.FirstOrDefaultAsync(x => product.IdentificadorExt == i.IdProducto && x.FechaIngreso == fechaIngreso, ct);
                if(existing == null) {
                    var inv = new Inventario
                    {
                        Sucursal = i.Sucursal,
                        IdProducto = product.IdProducto,
                        Cantidad = (decimal)i.Cantidad,
                        FechaIngreso = fechaIngreso
                    };

                    _app.Inventarios.Add(inv);
                }
                
            }

            await _app.SaveChangesAsync(ct);
        }

        public async Task SyncAllAsync(CancellationToken ct = default)
        {
            await SyncProductosAsync(ct);
            await SyncGastosAsync(ct);
            await SyncInventarioAsync(ct);
        }
    }
}
