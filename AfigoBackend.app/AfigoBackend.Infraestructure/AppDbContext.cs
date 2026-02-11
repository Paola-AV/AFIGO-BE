using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Domain.Pedido;
using AfigoBackend.Domain.Usuario;
using Microsoft.EntityFrameworkCore;

namespace AfigoBackend.Infraestructure
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Domain.Trabajador.Trabajador> Trabajadores { get; set; }
        public DbSet<Domain.PeticionVacaciones.PeticionVacaciones> PeticionesVacaciones { get; set; }

    }
}
