using AfigoBackend.Infraestructure.ExternalViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure
{
    public class ExternalDbContext : DbContext
    {
        public ExternalDbContext(DbContextOptions<ExternalDbContext> options) : base(options) { }

        public DbSet<ExternalProductoView> Productos { get; set; } = null!;
        public DbSet<ExternalGastoView> Gastos { get; set; } = null!;
        public DbSet<ExternalInventarioView> Inventarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExternalProductoView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Producto"); // nombre de la view en SQL Server
                eb.Property(p => p.IdProducto).HasColumnName("id_producto");
                eb.Property(p => p.Nombre).HasColumnName("nombre");
                eb.Property(p => p.Descripcion).HasColumnName("descripcion");
                eb.Property(p => p.PrecioCosto).HasColumnName("precio_costo");
                eb.Property(p => p.PrecioVenta).HasColumnName("precio_venta");
                eb.Property(p => p.Familia).HasColumnName("familia");
                eb.Property(p => p.Marca).HasColumnName("marca");
            });

            modelBuilder.Entity<ExternalGastoView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Gasto");
                eb.Property(g => g.Tipo).HasColumnName("tipo");
                eb.Property(g => g.Descripcion).HasColumnName("descripcion");
                eb.Property(g => g.Fecha).HasColumnName("fecha");
                eb.Property(g => g.Monto).HasColumnName("monto");
                eb.Property(g => g.Sucursal).HasColumnName("sucursal");
            });

            modelBuilder.Entity<ExternalInventarioView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Inventario");
                eb.Property(i => i.Sucursal).HasColumnName("sucursal");
                eb.Property(i => i.IdProducto).HasColumnName("id_producto");
                eb.Property(i => i.Cantidad).HasColumnName("cantidad");
                eb.Property(i => i.FechaIngreso).HasColumnName("fecha_ingreso");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
