using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Cliente;
using AfigoBackend.Domain.Proveedor;
using AfigoBackend.Domain.VentaDetalle;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class VentaDetalleService:IVentaDetalleInterface
    {
        private readonly AppDbContext _db;
        public VentaDetalleService(AppDbContext db) => _db = db;

        public Task<List<VentaDetalle>> GetAllAsync()
          => _db.VentaDetalles.AsNoTracking().ToListAsync();

        public Task<VentaDetalle?> GetByIdAsync(int id)
          => _db.VentaDetalles.FindAsync(id).AsTask();
    }
}
