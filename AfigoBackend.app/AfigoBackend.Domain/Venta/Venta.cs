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
        public DateOnly Fecha { get; set; }

        [Column("sucursal")]
        public string Sucursal { get; set; } = string.Empty;

        [Column("id_trabajador")]
        public int IdTrabajador { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("id_factura")]//cambiar en db
        public int IdFactura { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = string.Empty;

        [Column("monto_total")]
        public decimal montoTotal { get; set; }

        [Column("referencia")]
        public string Referencia { get; set; } = string.Empty;
    }
}
