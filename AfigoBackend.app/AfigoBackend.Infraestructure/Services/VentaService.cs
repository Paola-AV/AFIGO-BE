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
using DocumentFormat.OpenXml.Spreadsheet;

namespace AfigoBackend.Infraestructure.Services
{
    public class VentaService:IVentaInterface
    {
        private readonly AppDbContext _db;
        public VentaService(AppDbContext db) => _db = db;

        public Task<List<Venta>> GetAllAsync()
          => _db.Ventas.AsNoTracking().ToListAsync();


        public async Task<List<VentaGetDto>> GetVentasConDetallesAsync(DateTime desde, DateTime hasta)
        {
            var vendedorDict = await _db.Vendedores
                .AsNoTracking()
                .ToDictionaryAsync(v => (v.IdVendedorExt, v.IdBodega), v => v.Nombre);

            var ventas = await (
                from v in _db.Ventas.AsNoTracking()
                where v.Fecha >= desde && v.Fecha <= hasta
                orderby v.Fecha descending
                select new VentaGetDto
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Descripcion = v.Descripcion,
                    NombreVendor = null, 
                    NombreCliente = v.NombreCliente,
                    numFactura = v.numFactura,
                    Estado = v.Estado,
                    MontoTotal = v.MontoTotal,
                    Referencia = v.Referencia,
                    IdentificadorExt = v.IdentificadorExt,
                    IdVendedor = v.IdVendedor,      
                    IdBodegaVendedor = v.IdBodegaVendedor, 
                }
            ).ToListAsync();

            if (ventas.Count == 0) return ventas;

            foreach (var venta in ventas)
            {
                if (venta.IdVendedor.HasValue)
                {
                    vendedorDict.TryGetValue(
                        (venta.IdVendedor.Value, venta.IdBodegaVendedor),
                        out var nombre
                    );
                    venta.NombreVendor = nombre;
                }
            }

            var ids = ventas.Select(v => v.IdVenta).ToList();

            var detalles = await (
                from d in _db.VentaDetalles.AsNoTracking()
                join p in _db.Productos.AsNoTracking()
                     on d.IdProducto equals p.IdProducto
                where ids.Contains(d.IdVenta)
                select new VentaDetalleGetDto
                {
                    IdVentaDetalle = d.IdVentaDetalle,
                    IdVenta = d.IdVenta,
                    NombreProducto = p.Nombre,
                    FamiliaProducto = p.Familia,
                    Cantidad = d.Cantidad ?? 0
                }
            ).ToListAsync();

            var detallesPorVenta = detalles
                .GroupBy(d => d.IdVenta)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var venta in ventas)
                venta.VentaDetalles = detallesPorVenta.TryGetValue(venta.IdVenta, out var det)
                    ? det
                    : new List<VentaDetalleGetDto>();

            return ventas;
        }
        public async Task<List<VentaGetDto>> GetVentasConDetallesPorVendedorAsync(DateTime desde, DateTime hasta, string nombreVendedor)
        {
            var query =
                from v in _db.Ventas.AsNoTracking()
                join ven in _db.Vendedores.AsNoTracking()
                     on v.IdVendedor equals ven.IdVendedorExt into gv
                from ven in gv.DefaultIfEmpty()
                where v.Fecha >= desde && v.Fecha <= hasta
                      && ven != null && ven.Nombre == nombreVendedor  
                orderby v.Fecha descending
                select new VentaGetDto
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Descripcion = v.Descripcion,
                    NombreVendor = ven.Nombre,
                    NombreCliente = v.NombreCliente,
                    numFactura = v.numFactura,
                    Estado = v.Estado,
                    MontoTotal = v.MontoTotal,
                    Referencia = v.Referencia,
                    IdentificadorExt = v.IdentificadorExt,
                    VentaDetalles = _db.VentaDetalles
                        .AsNoTracking()
                        .Where(d => d.IdVenta == v.IdVenta)
                        .Join(
                            _db.Productos.AsNoTracking(),
                            d => d.IdProducto,
                            p => p.IdProducto,
                            (d, p) => new { d, p }
                        )
                        .Select(x => new VentaDetalleGetDto
                        {
                            IdVentaDetalle = x.d.IdVentaDetalle,
                            IdVenta = x.d.IdVenta,
                            NombreProducto = x.p.Nombre,
                            FamiliaProducto = x.p.Familia,
                            Cantidad = x.d.Cantidad ?? 0
                        })
                        .ToList()
                };
            return await query.ToListAsync();
        }

        public async Task<double> GetComisionMensualPorVendedorAsync(string nombreVendedor)
        {
            var hoy = DateTime.UtcNow;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var finMes = hoy;

            var totalVentas = await (
                from v in _db.Ventas.AsNoTracking()
                join ven in _db.Vendedores.AsNoTracking()
                     on v.IdVendedor equals ven.IdVendedorExt into gv
                from ven in gv.DefaultIfEmpty()
                where v.Fecha >= inicioMes
                      && v.Fecha <= finMes
                      && ven != null
                      && ven.Nombre == nombreVendedor
                select v.MontoTotal
            ).SumAsync();

            return (double)(totalVentas * (1.3 / 100));
        }

        public async Task<Dictionary<string, double>> GetAllComisionMensualPorVendedorAsync()
        {
            var hoy = DateTime.UtcNow;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var finMes = hoy;


            var vendedores = await _db.Vendedores
            .AsNoTracking()
            .Select(v => v.Nombre)
            .Distinct()
            .Where(n => n != null)
            .OrderBy(n => n)
            .ToListAsync();

            Dictionary<string, double> comisionesPorVendedor = new Dictionary<string, double>();
            foreach (var nombreVendedor in vendedores) {
                var totalVentas = await (
                   from v in _db.Ventas.AsNoTracking()
                   join ven in _db.Vendedores.AsNoTracking()
                        on v.IdVendedor equals ven.IdVendedorExt into gv
                   from ven in gv.DefaultIfEmpty()
                   where v.Fecha >= inicioMes
                         && v.Fecha <= finMes
                         && ven != null
                         && ven.Nombre == nombreVendedor
                   select v.MontoTotal
               ).SumAsync();
                var comision = (double)(totalVentas * (1.3/100));
                comisionesPorVendedor[nombreVendedor] = comision;
            }

               

            return comisionesPorVendedor;
        }

        public Task<List<VentaDTO>> GetByTrabajadorId(int id)//cambio
        {
            return _db.Ventas
                .AsNoTracking()
                .Where(v => v.IdVendedor == id)
                .Select(v => new VentaDTO
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Descripcion = v.Descripcion,
                    IdTrabajador = v.IdVendedor,
                    IdCliente = v.IdCliente,
                    numFactura = v.numFactura,
                    Estado = v.Estado,
                    MontoTotal = v.MontoTotal,
                    Referencia = v.Referencia,
                    NombreCliente = v.NombreCliente,
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

        public async Task<List<VentasDetallesExcelDto>> GetVentasParaExcelAsync(DateTime desde, DateTime hasta)
        {
            var ventas = await GetVentasConDetallesAsync(desde, hasta);
            var filas = new List<VentasDetallesExcelDto>();

            foreach (var venta in ventas)
            {
                var detalles = venta.VentaDetalles ?? new List<VentaDetalleGetDto>();

                if (detalles.Count == 0)
                {
                    // Venta sin detalles — una sola fila con detalles vacíos
                    filas.Add(MapearFila(venta, null));
                }
                else
                {
                    // Primera fila lleva los datos de la venta
                    filas.Add(MapearFila(venta, detalles[0], esPrimera: true));

                    // Filas subsiguientes — datos de venta en blanco
                    for (int i = 1; i < detalles.Count; i++)
                        filas.Add(MapearFila(null, detalles[i], esPrimera: false));
                }
            }

            return filas;
        }

        private static VentasDetallesExcelDto MapearFila(VentaGetDto? venta, VentaDetalleGetDto? detalle, bool esPrimera = true)
        {
            return new VentasDetallesExcelDto
            {
                // Datos de la venta solo en la primera fila del grupo
                NumFactura = esPrimera ? venta?.numFactura : null,
                Fecha = esPrimera ? venta?.Fecha : null,
                Descripcion = esPrimera ? venta?.Descripcion : null,
                Estado = esPrimera ? venta?.Estado : null,
                NombreVendedor = esPrimera ? venta?.NombreVendor : null,
                NombreCliente = esPrimera ? venta?.NombreCliente : null,
                Referencia = esPrimera ? venta?.Referencia : null,
                MontoTotal = esPrimera ? venta?.MontoTotal : null,

                // Detalle siempre visible
                NombreProducto = detalle?.NombreProducto,
                FamiliaProducto = detalle?.FamiliaProducto,
                Cantidad = detalle?.Cantidad,
            };
        }
    }
}
