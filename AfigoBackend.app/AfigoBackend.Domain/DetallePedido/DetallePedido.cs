using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Domain.Pedido;

namespace AfigoBackend.Domain.DetallePedido
{

    [Table("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        [Column("id_detalle")]
        public int IdDetalle { get; set; }

        [Column("id_pedido")]
        public int PedidoId { get; set; }

        [Column("nombre_producto")]
        [Required]
        public string NombreProducto { get; set; } = string.Empty;

        [Column("cant_producto")]
        public int CantProducto { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }


        [ForeignKey(nameof(PedidoId))]
        public Pedido.Pedido? Pedido { get; set; }

    }
}
