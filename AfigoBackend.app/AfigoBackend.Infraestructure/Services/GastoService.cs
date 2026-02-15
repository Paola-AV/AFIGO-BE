using AfigoBackend.Aplication.Abstractions.Interfaces;
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
    }
}
