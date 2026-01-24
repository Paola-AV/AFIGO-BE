using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Domain.Usuario;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IUsuarioInterface
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
        

    }
}
