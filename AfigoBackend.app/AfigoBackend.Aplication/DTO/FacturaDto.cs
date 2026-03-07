using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class FacturaDto
    {
        [Display(Name = "Proveedor")]
        public string? ProveedorNombre { get; set; }

        public string? Estado { get; set; } = string.Empty;

        public string? Sucursal { get; set; } = string.Empty;

        public DateTime? FechaFactura { get; set; }

        public string? Numero { get; set; } = string.Empty;
    }
}
