using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Producto
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("precio_costo")]
        public decimal PrecioCosto { get; set; }

        [Column("precio_venta")]
        public decimal PrecioVenta { get; set; }

        [Column("familia")]
        public string Familia { get; set; } = string.Empty;

        [Column("marca")]
        public string Marca { get; set; } = string.Empty;

        [Column("identificadorExt")]
        public string? IdentificadorExt { get; set; }
    }
}
