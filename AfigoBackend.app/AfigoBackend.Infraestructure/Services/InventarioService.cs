using AfigoBackend.Aplication.Abstractions.Interfaces;
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
    }
}
