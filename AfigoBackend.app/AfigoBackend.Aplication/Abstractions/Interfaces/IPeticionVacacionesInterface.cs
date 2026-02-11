using AfigoBackend.Domain.PeticionVacaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IPeticionVacacionesInterface
    {
        Task<PeticionVacaciones> CreateAsync(PeticionVacaciones peticion);
        Task<bool> DeleteAsync(int id);
        Task<List<PeticionVacaciones>> GetAllAsync();
        Task<PeticionVacaciones?> GetByIdAsync(int id);
        Task<List<PeticionVacaciones>> GetByIdTrabajador(int idTrabajador);
        Task<bool> UpdateAsync(PeticionVacaciones peticion);
    }
}
