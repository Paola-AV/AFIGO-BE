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

            var query =
                from v in _db.Ventas.AsNoTracking()
                join ven in _db.Vendedores.AsNoTracking()
                     on v.IdVendedor equals ven.IdVendedorExt into gv
                from ven in gv.DefaultIfEmpty() // left join vendedor

                    // --- OPCIONAL: Left join con Cliente (descomenta si tienes la entidad Cliente) ---
                    // join cli in _db.Clientes.AsNoTracking()
                    //      on v.IdCliente equals cli.IdCliente into gc
                    // from cli in gc.DefaultIfEmpty() // left join cliente
                where v.Fecha >= desde && v.Fecha <= hasta
                orderby v.Fecha descending
                select new VentaGetDto
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    Descripcion = v.Descripcion,
                    NombreVendor = ven != null ? ven.Nombre : null,
                    NombreCliente = null, 
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
                    NombreCliente = null,
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
