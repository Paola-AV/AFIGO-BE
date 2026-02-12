using AfigoBackend.Domain.Trabajador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface ITrabajadorInterface
    {
        Task<Trabajador> CreateAsync(Trabajador trabajador);
        Task<bool> DeleteAsync(int id);
        Task<List<Trabajador>> GetAllAsync();
        Task<List<Trabajador>> GetAllWithVacationDays();
        Task<Trabajador?> GetByIdAsync(int id);
        Task<Trabajador?> GetByUsuarioIdAsync(int idUsuario);
        Task<bool> UpdateAsync(Trabajador trabajador);
    }
}
