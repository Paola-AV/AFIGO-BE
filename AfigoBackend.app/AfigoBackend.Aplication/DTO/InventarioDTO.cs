using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class InventarioDTO
    {
        public string? Sucursal { get; set; } = string.Empty;
        public double? Cantidad { get; set; }
        [Display(Name = "Fecha Ingreso")]
        public DateTime? FechaIngreso { get; set; }
        [Display(Name = "Nombre Producto")]
        public string? NombreProducto { get; set; } = string.Empty;
        [Display(Name = "Familia Producto")]
        public string? FamiliaProducto { get; set; } = string.Empty;
        [Display(Name = "Marca Producto")]
        public string? MarcaProducto { get; set; } = string.Empty;

    }
}
