using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Gasto;
using AfigoBackend.Domain.Venta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class GastoService:IGastoInterface
    {
        private readonly AppDbContext _db;
        public GastoService(AppDbContext db) => _db = db;

        public Task<List<Gasto>> GetAllAsync()
          => _db.Gastos.AsNoTracking().ToListAsync();

        public async Task<List<GastoDto>> GetGastosParaExcel()
        {
            var query =
                from f in _db.Gastos.AsNoTracking()
                select new GastoDto
                {
                   Tipo = f.Tipo,
                   Descripcion = f.Descripcion,
                   Monto = f.Monto,
                   Fecha = f.Fecha,
                   Sucursal = f.Sucursal
                };

            return await query.ToListAsync();
        }
    }
}
