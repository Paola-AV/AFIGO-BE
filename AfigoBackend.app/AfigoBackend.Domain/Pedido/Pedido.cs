using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Pedido
{
    [Table("Pedido")]
    public class Pedido
    {

        [Key]
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Column("fecha_pedido")]
        public DateTime FechaPedido { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = string.Empty;

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre_cliente")]
        public string NombreCliente { get; set; } = string.Empty;

        [Column("factura_electronica")]
        public int FacturaElectronica { get; set; }

        [Column("detalle_factura")]
        public string? DetalleFactura { get; set; }

        [Column("metodo_envio")]
        public string MetodoEnvio { get; set; } = string.Empty;

        [Column("direccion_envio")]
        public string DireccionEnvio { get; set; } = string.Empty;

        [Column("urgencia_encio")]
        public string UrgenciaEnvio { get; set; } = string.Empty;

        [Column("tipo_pedido")]
        public string TipoPedido { get; set; } = string.Empty;

    }
}
