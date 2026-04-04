using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class PedidoConDetalleDto
    {
        public int IdPedido { get; set; }

        public DateOnly FechaPedido { get; set; }

        public string Estado { get; set; } = string.Empty;

        public string nombreVendedor { get; set; }

        public string NombreCliente { get; set; } = string.Empty;

        public int FacturaElectronica { get; set; }

        public string? DetalleFactura { get; set; }

        public string MetodoEnvio { get; set; } = string.Empty;

        public string DireccionEnvio { get; set; } = string.Empty;

        public string UrgenciaEnvio { get; set; } = string.Empty;

        public string TipoPedido { get; set; } = string.Empty;

        public string? Sucursal {  get; set; } = string.Empty;

        public List<DetallePedidoDto> Detalles { get; set; } = new List<DetallePedidoDto>();
    }

    public class DetallePedidoDto
    {
        public int IdDetalle { get; set; }

        public int PedidoId { get; set; }

        public string NombreProducto { get; set; } = string.Empty;

        public int CantProducto { get; set; }
        public string? Descripcion { get; set; }
    }
}
