using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Usuario;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IUsuarioInterface
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(int userId, int? trabajadorId, string correo, string nombreUsuario, string nombre, string? nombreVendedor, int isAdmin, int vendedor, string sede);
        Task<bool> DeleteAsync(int id);
        Task<List<UsuarioPerfilDto>> GetAllUsuarioTrabajadorAsync();
        Task<bool> InactivarUsuario(int userId);
    }
}
