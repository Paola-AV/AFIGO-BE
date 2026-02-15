using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Cuenta;
using AfigoBackend.Domain.Venta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class CuentaService:ICuentaInterface
    {
        private readonly AppDbContext _db;
        public CuentaService(AppDbContext db) => _db = db;

        public Task<List<Cuenta>> GetAllAsync()
          => _db.Cuentas.AsNoTracking().ToListAsync();
    }
}
