using AfigoControl.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;


namespace AfigoControl.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=Afigo_control.mssql.somee.com;Initial Catalog=Afigo_control;User ID=compartidoDev_SQLLogin_1;Password=47489168ny; Connection Timeout=200; pooling=true;Max Pool Size=32767;MultipleActiveResultSets=True");
        }
    }
}

