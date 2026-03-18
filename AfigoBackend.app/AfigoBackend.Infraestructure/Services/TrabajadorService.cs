using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Trabajador;
using AfigoBackend.Domain.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Infraestructure.Util;
using AfigoBackend.Domain.PeticionVacaciones;

namespace AfigoBackend.Infraestructure.Services
{
    public class TrabajadorService: ITrabajadorInterface
    {
        private readonly AppDbContext _db;
        private readonly IPeticionVacacionesInterface _peticionVacacionesService;

        public TrabajadorService(AppDbContext db, IPeticionVacacionesInterface peticionVacacionesService)
        {
            _db = db;
            _peticionVacacionesService = peticionVacacionesService;
        }
        


        public Task<List<Trabajador>> GetAllAsync()
            => _db.Trabajadores.AsNoTracking().ToListAsync();

        public async Task<List<Trabajador>> GetAllWithVacationDays()
        {
            List<Trabajador> trabajadores = await _db.Trabajadores.ToListAsync();
            if (trabajadores.Count == 0)
            {
                return trabajadores;
            }
            else
            {
                foreach (Trabajador trab in trabajadores)
                {

                    var peticiones = await _peticionVacacionesService.GetByIdTrabajador(trab.IdTrabajador);
                    int disponibles = VacacionesUtil.CalcularDiasVacacionesDisponibles(trab, peticiones);

                    trab.VacacionesDisponibles = disponibles;
                }
                return trabajadores;
            }
        }

        public async Task<Trabajador?> GetByIdAsync(int id)
        { 
            Trabajador trabajador = await _db.Trabajadores.FindAsync(id).AsTask();
            if(trabajador == null) { return null; }
            var peticiones = await _peticionVacacionesService.GetByIdTrabajador(trabajador.IdTrabajador);
            int disponible = VacacionesUtil.CalcularDiasVacacionesDisponibles(trabajador, peticiones);
            trabajador.VacacionesDisponibles = disponible;
            return trabajador;

        }
           
        

        public async Task<Trabajador?> GetByUsuarioIdAsync(int idUsuario)
        {
            Trabajador trabajador = await _db.Trabajadores.FirstOrDefaultAsync(t => t.IdUsuario == idUsuario);
            if (trabajador == null) { return null; }
            var peticiones = await _peticionVacacionesService.GetByIdTrabajador(trabajador.IdTrabajador);
            int disponible = VacacionesUtil.CalcularDiasVacacionesDisponibles(trabajador, peticiones);
            trabajador.VacacionesDisponibles = disponible;
            return trabajador;
        }
          

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
