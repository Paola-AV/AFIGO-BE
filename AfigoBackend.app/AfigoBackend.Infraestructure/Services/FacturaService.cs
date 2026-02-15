using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Domain.Factura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class FacturaService:IFacturaInterface
    {
        private readonly AppDbContext _db;
        public FacturaService(AppDbContext db) => _db = db;

        public Task<List<Factura>> GetAllAsync()
          => _db.Facturas.AsNoTracking().ToListAsync();
    }
}
