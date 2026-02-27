using AfigoBackend.Domain.VentaDetalle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class VentaDTO
    {
        public int IdVenta { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Descripcion { get; set; } 

        public int? IdTrabajador { get; set; }
        public int? IdCliente { get; set; }
        public string? numFactura { get; set; } 

        public string? Estado { get; set; }
        public double? MontoTotal { get; set; }

        public string? Referencia { get; set; } 

        public List<VentaDetalle> VentaDetalles { get; set; } = new();
    }
}
