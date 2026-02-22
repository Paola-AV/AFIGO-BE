
using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.DetallePedido;
using AfigoBackend.Infraestructure.Services;
using AfigoBackend.Infraestructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AfigoBackend.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUsuarioInterface, UsuarioService>();
            services.AddScoped<IPedidoInterface, PedidoService>();
            services.AddScoped<IDetallePedidoInterface, DetallePedidoService>();
            services.AddScoped<ITrabajadorInterface, TrabajadorService>();
            services.AddScoped<IPeticionVacacionesInterface, PeticionVacacionesService>();
            services.AddScoped<ICuentaInterface, CuentaService>();
            services.AddScoped <IInventarioInterface, InventarioService>();
            services.AddScoped <IGastoInterface, GastoService>();
            services.AddScoped <IFacturaInterface, FacturaService>();
            services.AddScoped <IVentaInterface, VentaService>();
            services.AddScoped <IProductoInterface, ProductoService>();
            services.AddScoped <IVentaDetalleInterface, VentaDetalleService>();
            services.AddScoped <IProveedorInterface, ProveedorService>();
            services.AddScoped <IClienteInterface, ClienteService>();
            services.AddScoped<IAuthInterface, AuthService>();
            services.AddScoped<IExternalSyncInterface, ExternalSyncService>();
            services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
            return services;
        }
    }
}
