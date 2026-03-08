using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    internal class UsuarioTrabajadorDto
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Correo { get; set; }
        public int UsuarioAdmin { get; set; }

        public string NombreDeUsuario { get; set; } = string.Empty;

        public string Contrasenia { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }

        public decimal VacacionesDisponibles { get; set; }
    }
}
