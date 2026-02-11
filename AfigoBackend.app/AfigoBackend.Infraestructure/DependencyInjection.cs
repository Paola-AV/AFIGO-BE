
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
            services.AddScoped<IAuthInterface, AuthService>();
            services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
            return services;
        }
    }
}
