using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class VentaGetDto
    {
        public int IdVenta { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Descripcion { get; set; } = string.Empty;

        public string? NombreVendor { get; set; }

        public string? NombreCliente { get; set; }

        public string? numFactura { get; set; } = string.Empty;

        public string? Estado { get; set; } = string.Empty;

        public double? MontoTotal { get; set; }

        public string? Referencia { get; set; } = string.Empty;

        public int IdentificadorExt { get; set; }

        public List<VentaDetalleGetDto> VentaDetalles { get; set; } = new List<VentaDetalleGetDto>();
    }

    public class VentaDetalleGetDto
    {
        public int IdVentaDetalle { get; set; }
        public int IdVenta { get; set; }
        public string? FamiliaProducto { get; set; }
        public string? NombreProducto { get; set; }
        public double Cantidad { get; set; }
    }
}
