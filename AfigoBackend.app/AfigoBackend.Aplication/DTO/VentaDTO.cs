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

        public DateOnly Fecha { get; set; }

        public string Sucursal { get; set; } = string.Empty;

        public int IdTrabajador { get; set; }
        public int IdCliente { get; set; }
        public string numFactura { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }

        public string Referencia { get; set; } = string.Empty;

        public List<VentaDetalle> VentaDetalles { get; set; } = new();
    }
}
