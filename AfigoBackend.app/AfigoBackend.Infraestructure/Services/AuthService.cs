using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Usuario;
using AfigoBackend.Infraestructure.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher _hasher;

        public AuthService(AppDbContext db, IPasswordHasher hasher)
            => (_db, _hasher) = (db, hasher);

        public async Task<bool> LoginAsync(string correoOUsuario, string password, CancellationToken ct)
        {
            var user = await _db.Set<Usuario>()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u =>
                                      u.Correo == correoOUsuario
                                   || u.NombreDeUsuario == correoOUsuario, ct);

            if (user is null) return false;

            return _hasher.Verify(user.Contrasenia, password);
        }

        public async Task RegistrarAsync(string correo, string nombre, string password, string nombreUsuario, int isAdmin, CancellationToken ct)
        {
            // Recomendado: evita duplicados
            var existe = await _db.Set<Usuario>()
                                  .AsNoTracking()
                                  .AnyAsync(u => u.Correo == correo, ct);
            if (existe) throw new InvalidOperationException("Correo ya está registrado.");

            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                NombreDeUsuario = nombreUsuario,
                Contrasenia = _hasher.Hash(password),
                UsuarioAdmin = isAdmin
            };

            await _db.AddAsync(usuario, ct);
            await _db.SaveChangesAsync(ct);


        }
    }
}
