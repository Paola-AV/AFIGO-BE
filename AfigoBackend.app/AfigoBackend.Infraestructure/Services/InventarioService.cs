using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Inventario;
using AfigoBackend.Domain.Venta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class InventarioService:IInventarioInterface
    {
        private readonly AppDbContext _db;
        public InventarioService(AppDbContext db) => _db = db;

        public Task<List<Inventario>> GetAllAsync()
          => _db.Inventarios.AsNoTracking().ToListAsync();

        public async Task<List<InventarioDTO>> GetInventariosParaExcel()
        {
            var query =
                from i in _db.Inventarios.AsNoTracking()
                join p in _db.Productos.AsNoTracking() on i.IdProducto equals p.IdProducto into pj
                from p in pj.DefaultIfEmpty() 
                select new InventarioDTO
                {
                    FechaIngreso = i.FechaIngreso,
                    Cantidad = i.Cantidad,
                    Sucursal = i.Sucursal,
                    NombreProducto = p != null ? p.Nombre : null,
                    FamiliaProducto = p != null ? p.Familia : null,
                    MarcaProducto = p != null ? p.Marca : null
                };

            return await query.ToListAsync();
        }
    }
}
