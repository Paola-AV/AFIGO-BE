using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Usuario;
using Microsoft.EntityFrameworkCore;


namespace AfigoBackend.Infraestructure.Services
{
    public class UsuarioService : IUsuarioInterface
    {

        private readonly AppDbContext _db;
        public UsuarioService(AppDbContext db) => _db = db;

        public Task<List<Usuario>> GetAllAsync()
            => _db.Usuarios.AsNoTracking().ToListAsync();

        public Task<Usuario?> GetByIdAsync(int id)
            => _db.Usuarios.FindAsync(id).AsTask();

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }
        public async Task<bool> UpdateAsync(Usuario usuario)
        {

            if (usuario is null || usuario.UserId <= 0) { return false; }
               

            var entity = await _db.Usuarios.FindAsync(usuario.UserId);
            if (entity is null) return false;

            entity.Nombre = usuario.Nombre;
            entity.Correo = usuario.Correo;
            entity.UsuarioAdmin = usuario.UsuarioAdmin;
            entity.NombreDeUsuario = usuario.NombreDeUsuario;
            entity.Contrasenia = usuario.Contrasenia;
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
