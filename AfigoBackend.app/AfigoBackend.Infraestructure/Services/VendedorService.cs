using AfigoBackend.Domain.Venta;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.Abstractions.Interfaces;

namespace AfigoBackend.Infraestructure.Services
{
    public class VendedorService: IVendedorInterface
    {
        private readonly AppDbContext _db;
        public VendedorService(AppDbContext db) => _db = db;


        public async Task<List<String>> GetAllNombres()
        {
            var vendedores = await _db.Vendedores
            .AsNoTracking()
            .Select(v => v.Nombre)
            .Distinct()
            .Where(n => n != null)
            .OrderBy(n => n)
            .ToListAsync();

            return vendedores;
        }
    }
}
