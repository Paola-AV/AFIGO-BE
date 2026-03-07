using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class CuentaDto
    {
        [Display(Name = "Proveedor")]
        public string? ProveedorNombre { get; set; }

        public double? Monto { get; set; }

        public double? Saldo { get; set; }

        public string? Estado { get; set; }
        [Display(Name = "Estado Factura")]
        public string? EstadoFactura { get; set; } = string.Empty;
        [Display(Name = "Sucursal")]
        public string? SucursalFactura { get; set; } = string.Empty;
        [Display(Name = "Fecha Factura")]
        public DateTime? FechaFactura { get; set; }
        [Display(Name = "Numero Factura")]
        public string? NumeroFactura { get; set; } = string.Empty;


    }
}
