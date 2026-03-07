using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class GastoDto
    {
        public string? Tipo { get; set; } = string.Empty;

        public string? Descripcion { get; set; } = string.Empty;

        public double? Monto { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Sucursal { get; set; } = string.Empty;
    }
}
