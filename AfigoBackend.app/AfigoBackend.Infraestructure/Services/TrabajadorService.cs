using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Trabajador;
using AfigoBackend.Domain.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class TrabajadorService: ITrabajadorInterface
    {
        private readonly AppDbContext _db;
        public TrabajadorService(AppDbContext db) => _db = db;

        public Task<List<Trabajador>> GetAllAsync()
            => _db.Trabajadores.AsNoTracking().ToListAsync();

        public Task<Trabajador?> GetByIdAsync(int id)
            => _db.Trabajadores.FindAsync(id).AsTask();

        public Task<Trabajador?> GetByUsuarioIdAsync(int idUsuario)
            => _db.Trabajadores.FirstOrDefaultAsync(t => t.IdUsuario == idUsuario);

        public async Task<Trabajador> CreateAsync(Trabajador trabajador) { 
            _db.Trabajadores.Add(trabajador);
            await _db.SaveChangesAsync();
            return trabajador;
        }

        public async Task<bool> UpdateAsync(Trabajador trabajador)
        {
            if (trabajador is null || trabajador.IdTrabajador <= 0) { return false; }
            
            var entity = await _db.Trabajadores.FindAsync(trabajador.IdTrabajador);
            if (entity is null) { return false; }

            entity.IdTrabajador = trabajador.IdTrabajador;
            entity.IdUsuario = trabajador.IdUsuario;
            entity.FechaInicio = trabajador.FechaInicio;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Trabajadores.FindAsync(id);
            if (entity is null) return false;
            _db.Trabajadores.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
