using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Cliente;
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
    public class ClienteService:IClienteInterface
    {
        private readonly AppDbContext _db;
        public ClienteService(AppDbContext db) => _db = db;

        public Task<List<Cliente>> GetAllAsync()
          => _db.Clientes.AsNoTracking().ToListAsync();

        public Task<Cliente?> GetByIdAsync(int id)
          => _db.Clientes.FindAsync(id).AsTask();
    }
}
