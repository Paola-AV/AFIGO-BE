using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Domain.Gasto;
using AfigoBackend.Domain.Proveedor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class ProveedorService : IProveedorInterface
    {
        private readonly AppDbContext _db;
        public ProveedorService(AppDbContext db) => _db = db;

        public Task<List<Proveedor>> GetAllAsync()
          => _db.Proveedores.AsNoTracking().ToListAsync();

        public Task<Proveedor?> GetByIdAsync(int id)
          => _db.Proveedores.FindAsync(id).AsTask();
    }
}
