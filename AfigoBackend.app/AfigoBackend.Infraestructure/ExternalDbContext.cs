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
        public DbSet<ExternalProveedorView> Proveedores { get; set; } = null!;
        public DbSet<ExternalFacturaView> Facturas { get; set; } = null!;
        public DbSet<ExternalVentaView> Ventas { get; set; } = null!;
        public DbSet<ExternalVentaDetalleView> VentaDetalles { get; set; } = null!;
        public DbSet<ExternalCuentaView> Cuentas { get; set; } = null!;
        public DbSet<ExternalVendedorView> Vendedores { get; set; }

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

            modelBuilder.Entity<ExternalProveedorView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Proveedor");
                eb.Property(p => p.IdProveedor).HasColumnName("id_proveedor");
                eb.Property(p => p.PrimerNombre).HasColumnName("primer_nombre");
                eb.Property(p => p.SegundoNombre).HasColumnName("segundo_nombre");
                eb.Property(p => p.PrimerApellido).HasColumnName("primer_apeliido");
                eb.Property(p => p.SegundoApellido).HasColumnName("segundo_apellido");
                eb.Property(p => p.CedulaFisica).HasColumnName("cedula_fisica");
                eb.Property(p => p.CedulaJuridica).HasColumnName("cedula_juridica");
                eb.Property(p => p.CorreoElectronico).HasColumnName("corrreo_electronico");
                eb.Property(p => p.Telefono).HasColumnName("telefono");
                eb.Property(p => p.Direccion).HasColumnName("direccion");
            });

            modelBuilder.Entity<ExternalFacturaView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Factura");
                eb.Property(f => f.IdFactura).HasColumnName("id_factura");
                eb.Property(f => f.Numero).HasColumnName("numero");
                eb.Property(f => f.Estado).HasColumnName("estado");
                eb.Property(f => f.Sucursal).HasColumnName("sucursal");
                eb.Property(f => f.Fecha).HasColumnName("fecha");
                eb.Property(f => f.IdProveedor).HasColumnName("id_proveedor");
            });

            modelBuilder.Entity<ExternalVentaView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Venta");
                eb.Property(v => v.IdVenta).HasColumnName("id_Venta");
                eb.Property(v => v.Fecha).HasColumnName("fecha");
                eb.Property(v => v.Descripcion).HasColumnName("descripcion");
                eb.Property(v => v.IdTrabajador).HasColumnName("id_trabajador");
                eb.Property(v => v.IdCliente).HasColumnName("id_cliente");
                eb.Property(v => v.NumFactura).HasColumnName("num_factura");
                eb.Property(v => v.Estado).HasColumnName("estado");
                eb.Property(v => v.MontoTotal).HasColumnName("montoTotal");
                eb.Property(v => v.Referencia).HasColumnName("referencia");
            });

            modelBuilder.Entity<ExternalVentaDetalleView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("VentaDetalle");
                eb.Property(d => d.IdVenta).HasColumnName("id_venta");
                eb.Property(d => d.IdProducto).HasColumnName("id_producto");
                eb.Property(d => d.cantidad).HasColumnName("cantidad");
            });

            modelBuilder.Entity<ExternalCuentaView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Cuenta");
                eb.Property(c => c.idProveedor).HasColumnName("id_proveedor");
                eb.Property(c => c.monto).HasColumnName("monto");
                eb.Property(c => c.idFactura).HasColumnName("id_factura");
                eb.Property(c => c.estado).HasColumnName("estado");
                eb.Property(c => c.saldo).HasColumnName("saldo");
            });

            modelBuilder.Entity<ExternalVendedorView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("Vendedores");
                eb.Property(c => c.IdVendedor).HasColumnName("id_vendedor");
                eb.Property(c => c.IdBodega).HasColumnName("id_bodega");
                eb.Property(c => c.Nombre).HasColumnName("nombre");
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
