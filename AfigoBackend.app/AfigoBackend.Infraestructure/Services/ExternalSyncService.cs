using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Cuenta;
using AfigoBackend.Domain.Factura;
using AfigoBackend.Domain.Gasto;
using AfigoBackend.Domain.Inventario;
using AfigoBackend.Domain.Producto;
using AfigoBackend.Domain.Proveedor;
using AfigoBackend.Domain.Venta;
using AfigoBackend.Domain.VentaDetalle;
using Microsoft.EntityFrameworkCore;

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
                        PrecioCosto = p.PrecioCosto,
                        PrecioVenta = p.PrecioVenta,
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
                    existing.PrecioCosto = p.PrecioCosto;
                    existing.PrecioVenta = p.PrecioVenta;
                    existing.Familia = p.Familia ?? existing.Familia;
                    existing.Marca = p.Marca ?? existing.Marca;
                }
            }

            if (_app.ChangeTracker.HasChanges())
                await _app.SaveChangesAsync(ct);
        }

        public async Task SyncGastosAsync(CancellationToken ct = default)
        {
            var cutoff = new DateTime(2025, 1, 1);
            var external = await _ext.Gastos.AsNoTracking().Where(g => g.Fecha >= cutoff).ToListAsync(ct);

            foreach (var g in external)
            {
                ct.ThrowIfCancellationRequested();
                var existing = await _app.Gastos.FirstOrDefaultAsync(x => x.Tipo == g.Tipo && x.Fecha == g.Fecha && x.Monto==g.Monto, ct);
                
                if ( existing==null)
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

        public async Task SyncInventarioAsync(CancellationToken ct = default)
        {
            var cutoff = new DateTime(2025, 1, 1);
            var external = await _ext.Inventarios.AsNoTracking().Where(i => i.FechaIngreso >= cutoff).ToListAsync(ct);

            foreach (var i in external)
            {
                // Buscar producto por Codigo (id_producto externo)
                var product = await _app.Productos.FirstOrDefaultAsync(p => p.IdentificadorExt == i.IdProducto, ct);
                if (product == null)
                {
                   
                    continue;
                }

                var existing = await _app.Inventarios.FirstOrDefaultAsync(x => product.IdentificadorExt == i.IdProducto && x.FechaIngreso == i.FechaIngreso && x.Sucursal==i.Sucursal && x.Cantidad==i.Cantidad, ct);
                if(existing == null) {
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

        public async Task SyncProveedoresAsync(CancellationToken ct = default)
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

        public async Task SyncFacturasAsync(CancellationToken ct = default)
        {
            var cutoff = new DateTime(2025, 1, 1);
            var external = await _ext.Facturas.AsNoTracking().Where(i => i.Fecha >= cutoff).ToListAsync(ct);

            foreach (var f in external)
            {
                ct.ThrowIfCancellationRequested();
                // Normalizar id_proveedor: quitar espacios, eliminar cualquier no dígito,
                // quitar ceros a la izquierda y convertir a int. Si no es convertible, saltar.
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

        public async Task SyncVentasAsync(CancellationToken ct = default)
        {
            var external = await _ext.Ventas.AsNoTracking().ToListAsync(ct);

            foreach (var v in external)
            {
                ct.ThrowIfCancellationRequested();

                var extId = v.IdVenta;

                var existing = await _app.Ventas.FirstOrDefaultAsync(x => x.IdentificadorExt == extId, ct);

                if (existing == null)
                {
                    var venta = new Venta
                    {
                        IdentificadorExt = extId,
                        Fecha = v.Fecha.HasValue ? v.Fecha.Value : default,
                        Descripcion = v.Descripcion ?? string.Empty,
                        IdTrabajador = v.IdTrabajador ?? 0,
                        IdCliente = v.IdCliente ?? 0,
                        numFactura = v.NumFactura ?? string.Empty,
                        Estado = v.Estado ?? string.Empty,
                        MontoTotal = v.MontoTotal,
                        Referencia = v.Referencia ?? string.Empty
                    };

                    _app.Ventas.Add(venta);
                }
                else
                {
                    existing.Fecha = v.Fecha.HasValue ? v.Fecha.Value : existing.Fecha;
                    existing.Descripcion = v.Descripcion ?? existing.Descripcion;
                    existing.IdTrabajador = v.IdTrabajador ?? existing.IdTrabajador;
                    existing.IdCliente = v.IdCliente ?? existing.IdCliente;
                    existing.numFactura = v.NumFactura ?? existing.numFactura;
                    existing.Estado = v.Estado ?? existing.Estado;
                    existing.MontoTotal = v.MontoTotal;
                    existing.Referencia = v.Referencia ?? existing.Referencia;
                }
            }

            await _app.SaveChangesAsync(ct);
        }

        public async Task SyncVentaDetallesAsync(CancellationToken ct = default)
        {
            var external = await _ext.VentaDetalles.AsNoTracking().ToListAsync(ct);

            foreach (var vd in external)
            {
                ct.ThrowIfCancellationRequested();

                // Buscar venta interna por IdentificadorExt == id_venta externo
                var venta = await _app.Ventas.FirstOrDefaultAsync(v => v.IdentificadorExt == vd.IdVenta, ct);
                if (venta == null) continue;

                // Buscar producto interno por IdentificadorExt == id_producto externo
                var producto = await _app.Productos.FirstOrDefaultAsync(p => p.IdentificadorExt == vd.IdProducto, ct);
                if (producto == null) continue;

                // comprobar existencia de detalle
                var existing = await _app.VentaDetalles.FirstOrDefaultAsync(d => d.IdVenta == venta.IdVenta && d.IdProducto == producto.IdProducto, ct);

                var cantidadInt = Convert.ToInt32(vd.cantidad ?? 0.0);

                if (existing == null)
                {
                    var detalle = new VentaDetalle
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = producto.IdProducto,
                        Cantidad = cantidadInt
                    };
                    _app.VentaDetalles.Add(detalle);
                }
                else
                {
                    existing.Cantidad = cantidadInt;
                }
            }

            await _app.SaveChangesAsync(ct);
        }

        public async Task SyncCuentasAsync(CancellationToken ct = default)
        {
            var external = await _ext.Cuentas.AsNoTracking().ToListAsync(ct);

            foreach (var c in external)
            {
                ct.ThrowIfCancellationRequested();

                // intentar resolver factura interna por IdentificadorExt = id_factura externo (si viene)
                int idFacturaInternal = 0;

                int? extFacturaId = null;
                if (!string.IsNullOrWhiteSpace(c.idFactura) && int.TryParse(c.idFactura, out var parsedId))
                {
                    extFacturaId = parsedId;
                }

                if (extFacturaId.HasValue)
                {
                    var factura = await _app.Facturas.FirstOrDefaultAsync(f => f.IdentificadorExt == extFacturaId.Value, ct);
                    if (factura != null) idFacturaInternal = factura.IdFactura;
                }

                // buscar si ya hay cuenta para el proveedor + factura (o crear nueva)
                var existing = await _app.Cuentas.FirstOrDefaultAsync(x => x.IdProveedor == c.idProveedor && x.IdFactura == idFacturaInternal, ct);

                if (existing == null)
                {
                    var cuenta = new Cuenta
                    {
                        IdProveedor = c.idProveedor,
                        Monto = c.monto,
                        IdFactura = idFacturaInternal,
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

        public async Task SyncAllAsync(CancellationToken ct = default)
        {
            await SyncProductosAsync(ct);
            await SyncGastosAsync(ct);
            await SyncInventarioAsync(ct);
            await SyncProveedoresAsync(ct);
            await SyncFacturasAsync(ct);
            await SyncVentasAsync(ct);
            await SyncVentaDetallesAsync(ct);
            await SyncCuentasAsync(ct);
        }
    }
}
