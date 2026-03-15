using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Cuenta;
using AfigoBackend.Domain.Factura;
using AfigoBackend.Domain.Gasto;
using AfigoBackend.Domain.Inventario;
using AfigoBackend.Domain.Producto;
using AfigoBackend.Domain.Proveedor;
using AfigoBackend.Domain.Sincronizacion;
using AfigoBackend.Domain.Vendedor;
using AfigoBackend.Domain.Venta;
using AfigoBackend.Domain.VentaDetalle;
using AfigoBackend.Infraestructure.ExternalViews;
using AfigoBackend.Infraestructure.Util;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;

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
            var mensaje = "Exitoso";
            try
            {
                var external = await _ext.Productos.AsNoTracking().ToListAsync(ct);
                if (external.Count == 0) return;

                // Solo con IdProducto válido
                var extIds = external
                    .Select(p => p.IdProducto)
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Distinct()
                    .ToList()!;

                var existentes = await _app.Productos
                    .Where(p => extIds.Contains(p.IdentificadorExt))
                    .ToListAsync(ct);

                var map = existentes.ToDictionary(p => p.IdentificadorExt);

                var nuevos = new List<Producto>();

                var prev = _app.ChangeTracker.AutoDetectChangesEnabled;
                _app.ChangeTracker.AutoDetectChangesEnabled = false;
                try
                {
                    foreach (var p in external)
                    {
                        ct.ThrowIfCancellationRequested();
                        if (string.IsNullOrWhiteSpace(p.IdProducto)) continue;

                        if (!map.TryGetValue(p.IdProducto!, out var existing))
                        {
                            var nuevo = new Producto
                            {
                                IdentificadorExt = p.IdProducto!,
                                Nombre = p.Nombre ?? string.Empty,
                                Descripcion = p.Descripcion ?? string.Empty,
                                PrecioCosto = p.PrecioCosto,
                                PrecioVenta = p.PrecioVenta,
                                Familia = p.Familia ?? string.Empty,
                                Marca = p.Marca ?? string.Empty
                            };
                            nuevos.Add(nuevo);
                            map[p.IdProducto!] = nuevo;
                        }
                        else
                        {
                            existing.Nombre = p.Nombre ?? existing.Nombre;
                            existing.Descripcion = p.Descripcion ?? existing.Descripcion;
                            existing.PrecioCosto = p.PrecioCosto;
                            existing.PrecioVenta = p.PrecioVenta;
                            existing.Familia = p.Familia ?? existing.Familia;
                            existing.Marca = p.Marca ?? existing.Marca;
                        }
                    }

                    if (nuevos.Count > 0)
                        _app.Productos.AddRange(nuevos);

                    if (_app.ChangeTracker.HasChanges())
                        await _app.SaveChangesAsync(ct);
                    
                }
                finally
                {
                    _app.ChangeTracker.AutoDetectChangesEnabled = prev;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncProductosAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Productos, mensaje, ct);
            }
        }

        public async Task SyncGastosAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var cutoff = new DateTime(2025, 1, 1);
                var external = await _ext.Gastos.AsNoTracking().Where(g => g.Fecha >= cutoff).ToListAsync(ct);

                foreach (var g in external)
                {
                    ct.ThrowIfCancellationRequested();
                    var existing = await _app.Gastos.FirstOrDefaultAsync(x => x.Tipo == g.Tipo && x.Fecha == g.Fecha && x.Monto == g.Monto, ct);

                    if (existing == null)
                    {
                        var gasto = new Gasto
                        {
                            Tipo = g.Tipo,
                            Descripcion = g.Descripcion,
                            Monto = g.Monto,
                            Fecha = g.Fecha,
                            Sucursal = g.Sucursal
                        };
                        _app.Gastos.Add(gasto);
                    }
                }

                if (_app.ChangeTracker.HasChanges())
                    await _app.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncGastosAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Gastos, mensaje, ct);
            }
        }

        public async Task SyncInventarioAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var cutoff = new DateTime(2025, 1, 1);
                var external = await _ext.Inventarios.AsNoTracking().Where(i => i.FechaIngreso >= cutoff).ToListAsync(ct);

                foreach (var i in external)
                {

                    var product = await _app.Productos.FirstOrDefaultAsync(p => p.IdentificadorExt == i.IdProducto, ct);
                    if (product == null)
                    {

                        continue;
                    }

                    var existing = await _app.Inventarios.FirstOrDefaultAsync(x => product.IdentificadorExt == i.IdProducto && x.FechaIngreso == i.FechaIngreso && x.Sucursal == i.Sucursal && x.Cantidad == i.Cantidad, ct);
                    if (existing == null)
                    {
                        var inv = new Inventario
                        {
                            Sucursal = i.Sucursal,
                            IdProducto = product.IdProducto,
                            Cantidad = i.Cantidad,
                            FechaIngreso = i.FechaIngreso
                        };

                        _app.Inventarios.Add(inv);
                    }

                }

                if (_app.ChangeTracker.HasChanges())
                    await _app.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncInventarioAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Inventarios, mensaje, ct);
            }
        }

        public async Task SyncProveedoresAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var external = await _ext.Proveedores.AsNoTracking().ToListAsync(ct);

                foreach (var p in external)
                {
                    ct.ThrowIfCancellationRequested();

                    var existing = await _app.Proveedores.FirstOrDefaultAsync(x => x.IdentificadorExt == p.IdProveedor, ct);

                    if (existing == null)
                    {
                        var nuevo = new Proveedor
                        {
                            IdentificadorExt = p.IdProveedor,
                            PrimerNombre = p.PrimerNombre ?? string.Empty,
                            SegundoNombre = p.SegundoNombre ?? string.Empty,
                            PrimerApellido = p.PrimerApellido ?? string.Empty,
                            SegundoApellido = p.SegundoApellido ?? string.Empty,
                            CedulaFisica = p.CedulaFisica ?? string.Empty,
                            CedulaJuridica = p.CedulaJuridica,
                            CorreoElectronico = p.CorreoElectronico,
                            Telefono = p.Telefono,
                            Direccion = p.Direccion
                        };
                        _app.Proveedores.Add(nuevo);
                    }
                    else
                    {
                        existing.PrimerNombre = p.PrimerNombre ?? existing.PrimerNombre;
                        existing.SegundoNombre = p.SegundoNombre ?? existing.SegundoNombre;
                        existing.PrimerApellido = p.PrimerApellido ?? existing.PrimerApellido;
                        existing.SegundoApellido = p.SegundoApellido ?? existing.SegundoApellido;
                        existing.CedulaFisica = p.CedulaFisica ?? existing.CedulaFisica;
                        existing.CedulaJuridica = p.CedulaJuridica ?? existing.CedulaJuridica;
                        existing.CorreoElectronico = p.CorreoElectronico ?? existing.CorreoElectronico;
                        existing.Telefono = p.Telefono ?? existing.Telefono;
                        existing.Direccion = p.Direccion ?? existing.Direccion;
                    }
                }

                await _app.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncProveedoresAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Proveedores, mensaje, ct);
            }
        }

        public async Task SyncFacturasAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var cutoff = new DateTime(2025, 1, 1);
                var external = await _ext.Facturas.AsNoTracking().Where(i => i.Fecha >= cutoff).ToListAsync(ct);

                foreach (var f in external)
                {
                    ct.ThrowIfCancellationRequested();

                    var raw = f.IdProveedor ?? string.Empty;
                    raw = raw.Trim();
                    var digitsOnly = System.Text.RegularExpressions.Regex.Replace(raw, @"\D", "");
                    var cleaned = digitsOnly.TrimStart('0');
                    if (string.IsNullOrEmpty(cleaned))
                        cleaned = "0";

                    if (!int.TryParse(cleaned, out var proveedorExtId))
                        continue;


                    var proveedor = await _app.Proveedores.FirstOrDefaultAsync(p => p.IdentificadorExt == proveedorExtId, ct);
                    if (proveedor == null)
                        continue;


                    var extId = f.IdFactura;

                    var existing = await _app.Facturas.FirstOrDefaultAsync(x => x.IdentificadorExt == extId, ct);

                    if (existing == null)
                    {
                        var factura = new Factura
                        {
                            IdentificadorExt = extId,
                            Numero = f.Numero,
                            Estado = f.Estado,
                            Sucursal = f.Sucursal,
                            Fecha = f.Fecha,
                            IdProveedor = proveedor.IdProveedor
                        };

                        _app.Facturas.Add(factura);
                    }

                }

                await _app.SaveChangesAsync(ct);

               
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncFacturasAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Facturas, mensaje, ct);
            }
        }

        public async Task SyncVendedoresAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var external = await _ext.Vendedores.AsNoTracking().ToListAsync(ct);

                // La fuente externa tiene duplicados 
                var externalDedup = external
                    .GroupBy(v => (v.IdVendedor, v.IdBodega))
                    .Select(g => g.First())
                    .ToList();

                foreach (var v in externalDedup)
                {
                    ct.ThrowIfCancellationRequested();

                    var existing = await _app.Vendedores
                        .FirstOrDefaultAsync(x => x.IdVendedorExt == v.IdVendedor
                                               && x.IdBodega == v.IdBodega, ct);
                    if (existing == null)
                    {
                        _app.Vendedores.Add(new Vendedor
                        {
                            IdBodega = v.IdBodega,
                            IdVendedorExt = v.IdVendedor,
                            Nombre = v.Nombre
                        });
                    }
                    else
                    {
                        existing.Nombre = v.Nombre;
                    }
                }

                await _app.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncVendedoresAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Vendedores, mensaje, ct);
            }
        }

        public async Task SyncVentasAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var cutoff = new DateTime(2025, 1, 1);

                var external = await _ext.Ventas.AsNoTracking().Where(i => i.Fecha >= cutoff).ToListAsync(ct);

                if (external.Count == 0)
                    return;

                var processedExtIds = external.Select(v => v.IdVenta).Distinct().ToList();

                var existingVentas = await _app.Ventas.Where(x => processedExtIds.Contains(x.IdentificadorExt)).ToListAsync(ct);

                var existingByExtId = existingVentas.ToDictionary(v => v.IdentificadorExt);

                foreach (var v in external)
                {
                    ct.ThrowIfCancellationRequested();
                    if (v.IdVenta == null || v.IdVenta == 0) continue;

                    var extId = v.IdVenta;

                    var desc = v.Descripcion ?? string.Empty;
                    var idBodega = 0;
                    if (desc.Contains("PALMARES")) idBodega = 1;
                    else if (desc.Contains("NICOYA")) idBodega = 2;
                    else if (desc.Contains("SARCHI")) idBodega = 3; 

                    if (!existingByExtId.TryGetValue(extId, out var existing))
                    {
                        var venta = new Venta
                        {
                            IdentificadorExt = extId,
                            Fecha = v.Fecha ?? default,
                            Descripcion = v.Descripcion ?? string.Empty,
                            IdVendedor = v.IdTrabajador ?? null,
                            IdCliente = v.IdCliente ?? null,
                            numFactura = v.NumFactura ?? string.Empty,
                            Estado = v.Estado ?? string.Empty,
                            MontoTotal = v.MontoTotal,
                            Referencia = v.Referencia ?? string.Empty,
                            IdBodegaVendedor= idBodega
                        };

                        _app.Ventas.Add(venta);
                        existingByExtId[extId] = venta;
                    }
                    else
                    {
                        if (v.Fecha.HasValue) existing.Fecha = v.Fecha.Value;
                        if (v.Descripcion != null) existing.Descripcion = v.Descripcion;
                        if (v.IdTrabajador.HasValue) existing.IdVendedor = v.IdTrabajador.Value;
                        if (v.IdCliente.HasValue) existing.IdCliente = v.IdCliente.Value;
                        if (v.NumFactura != null) existing.numFactura = v.NumFactura;
                        if (v.Estado != null) existing.Estado = v.Estado;
                        existing.MontoTotal = v.MontoTotal;
                        if (v.Referencia != null) existing.Referencia = v.Referencia;
                        existing.IdBodegaVendedor = idBodega;
                    }
                }

                if (_app.ChangeTracker.HasChanges())
                    await _app.SaveChangesAsync(ct);

                if (processedExtIds.Count > 0)
                    await SyncVentaDetallesAsync(processedExtIds, ct);

            }
            catch (Exception ex) { 
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncVentasAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Ventas, mensaje, ct);
            }
        }

        public async Task SyncVentaDetallesAsync(IEnumerable<int>? extVentaIds, CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                if (extVentaIds == null)
                    return;

                var ventaIdsList = extVentaIds.Distinct().ToList();
                if (ventaIdsList.Count == 0)
                    return;

                var minId = ventaIdsList.Min();
                var maxId = ventaIdsList.Max();
                var ventaIdsSet = new HashSet<int>(ventaIdsList);

                var externalDetalles = (await _ext.VentaDetalles
                    .AsNoTracking()
                    .Where(d => d.IdVenta >= minId && d.IdVenta <= maxId)
                    .ToListAsync(ct))
                    .Where(d => ventaIdsSet.Contains(d.IdVenta))
                    .ToList();

                if (externalDetalles.Count == 0)
                    return;

                var ventasInternas = await _app.Ventas
                    .AsNoTracking()
                    .Where(v => ventaIdsList.Contains(v.IdentificadorExt))
                    .Select(v => new { v.IdentificadorExt, v.IdVenta })
                    .ToListAsync(ct);

                var ventaExtToIntId = ventasInternas.ToDictionary(v => v.IdentificadorExt, v => v.IdVenta);

                var extProductoIds = externalDetalles
                    .Select(d => d.IdProducto)
                    .Distinct()
                    .ToList();

                var productosInternos = await _app.Productos
                    .AsNoTracking()
                    .Where(p => extProductoIds.Contains(p.IdentificadorExt))
                    .Select(p => new { p.IdentificadorExt, p.IdProducto })
                    .ToListAsync(ct);

                var prodExtToIntId = productosInternos.ToDictionary(p => p.IdentificadorExt, p => p.IdProducto);

                var paresInternos = externalDetalles
                    .Select(d =>
                    {
                        var okVenta = ventaExtToIntId.TryGetValue(d.IdVenta, out var vInt);
                        var okProd = prodExtToIntId.TryGetValue(d.IdProducto, out var pInt);
                        return new
                        {
                            Ok = okVenta && okProd,
                            VentaIdInt = vInt,
                            ProductoIdInt = pInt,
                            CantidadOrigen = d.cantidad 
                        };
                    })
                    .Where(x => x.Ok)
                    .ToList();

                if (paresInternos.Count == 0)
                    return;

                var ventaIdsInt = paresInternos.Select(x => x.VentaIdInt).Distinct().ToList();
                var prodIdsInt = paresInternos.Select(x => x.ProductoIdInt).Distinct().ToList();

                var detallesExistentes = await _app.VentaDetalles
                    .Where(d => ventaIdsInt.Contains(d.IdVenta) && prodIdsInt.Contains(d.IdProducto))
                    .ToListAsync(ct);

                var detallesMap = detallesExistentes.ToDictionary(
                    d => (d.IdVenta, d.IdProducto),
                    d => d);

                var nuevos = new List<VentaDetalle>();

                foreach (var x in paresInternos)
                {
                    ct.ThrowIfCancellationRequested();

                    var cantidadInt = x.CantidadOrigen.HasValue
                        ? (int)Math.Floor(Convert.ToDecimal(x.CantidadOrigen.Value))
                        : 0;

                    var key = (x.VentaIdInt, x.ProductoIdInt);

                    if (!detallesMap.TryGetValue(key, out var existing))
                    {
                        var nuevo = new VentaDetalle
                        {
                            IdVenta = x.VentaIdInt,
                            IdProducto = x.ProductoIdInt,
                            Cantidad = cantidadInt
                        };
                        nuevos.Add(nuevo);
                        detallesMap[key] = nuevo;
                    }
                    else
                    {
                        existing.Cantidad = cantidadInt;
                    }
                }

                if (nuevos.Count > 0)
                    _app.VentaDetalles.AddRange(nuevos);

                await _app.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncVentaDetallesAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.VentaDetalles, mensaje, ct);
            }
        }

        public async Task SyncCuentasAsync(CancellationToken ct = default)
        {
            var mensaje = "Exitoso";
            try
            {
                var external = await _ext.Cuentas.AsNoTracking().ToListAsync(ct);

                foreach (var c in external)
                {
                    ct.ThrowIfCancellationRequested();

                    int? idFacturaInternal = null;

                    if (!string.IsNullOrWhiteSpace(c.idFactura) && int.TryParse(c.idFactura, out var parsedId))
                    {
                        var factura = await _app.Facturas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(f => f.IdentificadorExt == parsedId, ct);

                        if (factura == null)
                        {

                            continue;
                        }

                        idFacturaInternal = factura.IdFactura;
                    }

                    var proveedor = await _app.Proveedores
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.IdentificadorExt == c.idProveedor, ct);

                    if (proveedor == null)
                    {
                        continue;
                    }

                    var idProveedorInternal = proveedor.IdProveedor;

                    var existing = await _app.Cuentas.FirstOrDefaultAsync(
                        x => x.IdProveedor == idProveedorInternal && x.IdFactura == idFacturaInternal, ct);

                    if (existing == null)
                    {
                        var cuenta = new Cuenta
                        {
                            IdProveedor = idProveedorInternal,   
                            IdFactura = idFacturaInternal,      
                            Monto = c.monto,
                            Saldo = c.saldo,
                            Estado = c.estado
                        };

                        _app.Cuentas.Add(cuenta);
                    }
                    else
                    {
                        existing.Monto = c.monto;
                        existing.Saldo = c.saldo;
                        existing.Estado = c.estado ?? existing.Estado;
                    }
                }

                await _app.SaveChangesAsync(ct);

            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
                Console.WriteLine($"Error en SyncCuentasAsync: {ex.Message}");
            }
            finally
            {
                await GuardarSyncAsync(Constants.TiposSync.Cuentas, mensaje, ct);
            }
        }

        public async Task SyncAllAsync(CancellationToken ct = default)
        {
            await SyncProductosAsync(ct);
            await SyncProveedoresAsync(ct);
            await SyncGastosAsync(ct);
            await SyncInventarioAsync(ct);
            await SyncFacturasAsync(ct);
            await SyncVendedoresAsync(ct);
            await SyncVentasAsync(ct);
            await SyncCuentasAsync(ct);
        }

        public async Task SyncCuentas (CancellationToken ct = default)
        {
            await SyncFacturasAsync(ct);
            await SyncCuentasAsync(ct);
        }

        public async Task SyncVentas(CancellationToken ct = default)
        {
            await SyncVendedoresAsync (ct);
            await SyncVentasAsync(ct);
        }

        public async Task SyncFacturas(CancellationToken ct = default)
        {
            await SyncFacturasAsync(ct);
        }

        public async Task SyncInventario(CancellationToken ct = default)
        {
            await SyncInventarioAsync(ct);
        }

        public async Task SyncGasto(CancellationToken ct = default)
        {
            await SyncGastosAsync(ct);
        }

        public async Task SyncProveedores(CancellationToken ct = default)
        {
            await SyncProveedoresAsync(ct);
        }

        public async Task SyncProductos(CancellationToken ct = default)
        {
            await SyncProductosAsync(ct);
        }

        public Task<List<Sincronizacion>> GetAllSyncEstadosAsync()
          => _app.Sincronizaciones.AsNoTracking().ToListAsync();


        private async Task GuardarSyncAsync(string tipo, string mensaje, CancellationToken ct)
        {
            var existing = await _app.Sincronizaciones
                .FirstOrDefaultAsync(s => s.Tipo == tipo, ct);

            if (existing != null)
            {
                existing.UltimaFecha = DateTime.UtcNow;
                existing.Mensaje = mensaje;
                _app.Sincronizaciones.Update(existing);
            }
            else
            {
                _app.Sincronizaciones.Add(new Sincronizacion
                {
                    Tipo = tipo,
                    UltimaFecha = DateTime.UtcNow,
                    Mensaje = mensaje
                });
            }

            await _app.SaveChangesAsync(ct);
        }

    }
}
