using AfigoBackend.Domain.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IAuthInterface
    {
        Task<bool> LoginAsync(string correoOUsuario, string password, CancellationToken ct);
        Task RegistrarAsync(string correo, string nombre, string password, string nombreUsuario, int isAdmin, CancellationToken ct);
    }
}
