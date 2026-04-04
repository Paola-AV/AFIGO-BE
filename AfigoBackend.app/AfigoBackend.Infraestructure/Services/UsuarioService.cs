using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AfigoBackend.Infraestructure.Services
{
    public class UsuarioService : IUsuarioInterface
    {

        private readonly AppDbContext _db;
        public UsuarioService(AppDbContext db) => _db = db;

        public Task<List<Usuario>> GetAllAsync()
            => _db.Usuarios.AsNoTracking().ToListAsync();

        public Task<List<UsuarioPerfilDto>> GetAllUsuarioTrabajadorAsync()
        => _db.Usuarios
            .AsNoTracking()
            .Select(u => new UsuarioPerfilDto
            {
                UserId = u.UserId,
                Nombre = u.Nombre,
                Correo = u.Correo,
                NombreDeUsuario = u.NombreDeUsuario,
                UsuarioAdmin = u.UsuarioAdmin,
                Activo = u.Activo,
                Trabajador = _db.Trabajadores
                    .Where(t => t.IdUsuario == u.UserId)
                    .Select(t => new TrabajadorPerfilDto
                    {
                        IdTrabajador = t.IdTrabajador,
                        FechaInicio = t.FechaInicio,
                        VacacionesDisponibles = t.VacacionesDisponibles,
                        Vendedor = t.Vendedor,
                        NombreVendedor = t.NombreVendedor,
                        Sede=t.Sede
                    })
                    .FirstOrDefault()
            })
            .ToListAsync();

        public Task<Usuario?> GetByIdAsync(int id)
            => _db.Usuarios.FindAsync(id).AsTask();

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }
        public async Task<bool> UpdateAsync(int userId, int? trabajadorId, string correo, string nombreUsuario, string nombre, string? nombreVendedor, int isAdmin, int vendedor, string sede)
        {
            var entity = await _db.Usuarios.FindAsync(userId);
            if (entity is null) return false;

            entity.Nombre = nombre;
            entity.Correo = correo;
            entity.UsuarioAdmin = isAdmin;
            entity.NombreDeUsuario = nombreUsuario;

            if (trabajadorId.HasValue)
            {
                var trabajador = await _db.Trabajadores.FindAsync(trabajadorId.Value);
                if (trabajador != null)
                {
                    trabajador.NombreVendedor = nombreVendedor;
                    trabajador.Vendedor = vendedor;
                    trabajador.Sede = sede;
                }
            }

            await _db.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> InactivarUsuario(int userId)
        {
            var entity = await _db.Usuarios.FindAsync(userId);
            if (entity is null) return false;
            entity.Activo = 0; 
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Usuarios.FindAsync(id);
            if (entity is null) return false;

            _db.Usuarios.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
