using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class UsuarioPerfilDto
    {
        public int UserId { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public string? Correo { get; set; }

        public int UsuarioAdmin { get; set; }

        public string NombreDeUsuario { get; set; } = string.Empty;

        public string Contrasenia { get; set; } = string.Empty;

        public int Activo { get; set; }

        public TrabajadorPerfilDto Trabajador { get; set; } = new TrabajadorPerfilDto();
    }
    public class TrabajadorPerfilDto
    {

        public int IdTrabajador { get; set; }

        public int IdUsuario { get; set; }

        public DateOnly FechaInicio { get; set; }

        public decimal VacacionesDisponibles { get; set; }

        public int? Vendedor { get; set; }

        public string? NombreVendedor { get; set; }

        public string? Sede { get; set; }
    }
}
