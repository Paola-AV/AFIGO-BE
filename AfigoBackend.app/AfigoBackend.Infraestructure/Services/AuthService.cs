using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Usuario;
using AfigoBackend.Domain.Trabajador;
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

        public async Task<Usuario?> LoginAsync(string correoOUsuario, string password, CancellationToken ct)
        {
            var user = await _db.Set<Usuario>()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u =>
                                      u.Correo == correoOUsuario
                                   || u.NombreDeUsuario == correoOUsuario, ct);

            if (user is null) return null;
            if (user.Activo == 0) return null;
            var ok = _hasher.Verify(user.Contrasenia, password);
            return ok ? user : null;
        }



        public async Task RegistrarAsync(string correo, string nombre, string password, string nombreUsuario, int isAdmin, CancellationToken ct)
        {

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
                UsuarioAdmin = isAdmin,
                Activo = 1
            };

            await _db.AddAsync(usuario, ct);
            await _db.SaveChangesAsync(ct);


        }

        public async Task RegistrarUsuarioTrabajadorAsync(string correo, string nombre, string password, string nombreUsuario, int isAdmin,DateOnly fechaInicio, decimal vacacionesDisponibles, int vendedor, string nombreVendedor, string sede, CancellationToken ct)
        {

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
                UsuarioAdmin = isAdmin,
                Activo=1
            };

            var User=await _db.AddAsync(usuario, ct);
            await _db.SaveChangesAsync(ct);

            var trabajador = new Trabajador
            {
                IdUsuario = usuario.UserId, 
                FechaInicio = fechaInicio,
                VacacionesDisponibles = vacacionesDisponibles,
                Vendedor = vendedor,
                NombreVendedor = nombreVendedor,
                Sede =sede
            };

            await _db.AddAsync(trabajador, ct);
            await _db.SaveChangesAsync(ct);


        }

        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken ct)
        {
            var user = await _db.Set<Usuario>()
                                .FirstOrDefaultAsync(u => u.UserId == userId, ct);

            if (user is null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            var currentOk = _hasher.Verify(user.Contrasenia, currentPassword);
            if (!currentOk)
                throw new UnauthorizedAccessException("Contraseña actual incorrecta.");

            var sameAsCurrent = _hasher.Verify(user.Contrasenia, newPassword);
            if (sameAsCurrent)
                throw new InvalidOperationException("La nueva contraseña no puede ser igual a la actual.");

            ValidatePassword(newPassword);

            user.Contrasenia = _hasher.Hash(newPassword);
            await _db.SaveChangesAsync(ct);
        }

        public async Task ChangePasswordAsyncForce(int userId, string newPassword, CancellationToken ct)
        {
            var user = await _db.Set<Usuario>()
                                .FirstOrDefaultAsync(u => u.UserId == userId, ct);

            if (user is null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            
            ValidatePassword(newPassword);

            user.Contrasenia = _hasher.Hash(newPassword);
            await _db.SaveChangesAsync(ct);
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");

        }
    }
}
