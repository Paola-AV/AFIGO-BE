using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Factura;
using AfigoBackend.Domain.Venta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class VentaService:IVentaInterface
    {
        private readonly AppDbContext _db;
        public VentaService(AppDbContext db) => _db = db;

        public Task<List<Venta>> GetAllAsync()
          => _db.Ventas.AsNoTracking().ToListAsync();
    }
}
