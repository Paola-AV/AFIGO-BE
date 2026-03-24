using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.PeticionVacaciones;
using AfigoBackend.Domain.Trabajador;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class PeticionVacacionesService: IPeticionVacacionesInterface
    {
        private readonly AppDbContext _db;
        public PeticionVacacionesService(AppDbContext db) => _db = db;

        public Task<List<PeticionVacaciones>> GetAllAsync()
          => _db.PeticionesVacaciones.AsNoTracking().ToListAsync();

        public Task <List<PeticionVacaciones>> GetAllOnFuture()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var peticiones= _db.PeticionesVacaciones.AsNoTracking().Where(p => p.FechaInicio > hoy).ToListAsync();
            return peticiones;
        }

        public Task<List<PeticionVacaciones>> GetAllOnPast()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var peticiones = _db.PeticionesVacaciones.AsNoTracking().Where(p => p.FechaInicio <= hoy).ToListAsync();
            return peticiones;
        }

        public Task<PeticionVacaciones?> GetByIdAsync(int id)
          => _db.PeticionesVacaciones.FindAsync(id).AsTask();

        public Task<List<PeticionVacaciones>> GetByIdTrabajador(int idTrabajador)
                => _db.PeticionesVacaciones
                    .AsNoTracking()
                    .Where(p => p.IdTrabajador == idTrabajador)
                    .ToListAsync();
        public async Task<PeticionVacaciones> CreateAsync(PeticionVacaciones peticion)
        {
            peticion.FechaCreado = DateTime.UtcNow;
            _db.PeticionesVacaciones.Add(peticion);
            await _db.SaveChangesAsync();
            return peticion;
        }

        public async Task<bool> UpdateAsync(PeticionVacaciones peticion)
        {
            if (peticion is null || peticion.IdPeticion <= 0) { return false; }
            var entity = await _db.PeticionesVacaciones.FindAsync(peticion.IdPeticion);
            if (entity is null) { return false; }
            entity.IdTrabajador = peticion.IdTrabajador;
            entity.FechaInicio = peticion.FechaInicio;
            entity.FechaFin = peticion.FechaFin;
            entity.Estado = peticion.Estado;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.PeticionesVacaciones.FindAsync(id);
            if (entity is null) return false;
            _db.PeticionesVacaciones.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
