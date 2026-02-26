using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Venta
{
    [Table("Venta")]
    public class Venta
    {
        [Key]
        [Column("id_venta")]
        public int IdVenta { get; set; }

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; } = string.Empty;

        [Column("id_trabajador")]
        public int? IdTrabajador { get; set; }

        [Column("id_cliente")]
        public int? IdCliente { get; set; }

        [Column("num_factura")]//cambiar en db
        public string? numFactura { get; set; } = string.Empty;

        [Column("estado")]
        public string? Estado { get; set; } = string.Empty;

        [Column("montoTotal")]
        public double? MontoTotal { get; set; }
         
        [Column("referencia")]
        public string? Referencia { get; set; } = string.Empty;

        [Column("identificadorExt")]
        public int? IdentificadorExt { get; set; }
    }
}
