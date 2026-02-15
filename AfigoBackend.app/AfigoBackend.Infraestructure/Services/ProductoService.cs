using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Cliente;
using AfigoBackend.Domain.Producto;
using AfigoBackend.Domain.Proveedor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class ProductoService:IProductoInterface
    {
        private readonly AppDbContext _db;
        public ProductoService(AppDbContext db) => _db = db;

        public Task<List<Producto>> GetAllAsync()
          => _db.Productos.AsNoTracking().ToListAsync();

        public Task<Producto?> GetByIdAsync(int id)
          => _db.Productos.FindAsync(id).AsTask();
    }
}
