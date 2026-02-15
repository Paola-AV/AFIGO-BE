using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Domain.Cuenta;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Domain.Factura;
using AfigoBackend.Domain.Gasto;
using AfigoBackend.Domain.Inventario;
using AfigoBackend.Domain.Pedido;
using AfigoBackend.Domain.Usuario;
using AfigoBackend.Domain.Venta;
using AfigoBackend.Domain.VentaDetalle;
using AfigoBackend.Domain.Producto;
using AfigoBackend.Domain.Cliente;
using AfigoBackend.Domain.Proveedor;
using Microsoft.EntityFrameworkCore;
using AfigoBackend.Domain.Trabajador;
using AfigoBackend.Domain.PeticionVacaciones;

namespace AfigoBackend.Infraestructure
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }
        public DbSet<PeticionVacaciones> PeticionesVacaciones { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }


    }
}
