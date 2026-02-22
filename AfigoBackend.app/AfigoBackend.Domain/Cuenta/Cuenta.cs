using AfigoBackend.Domain.Pedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Cuenta
{
    [Table("Cuenta")]
    public class Cuenta
    {
        [Key]
        [Column("id_cuenta")]
        public int IdCuenta { get; set; }

        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Column("monto")]
        public decimal Monto { get; set; }

        [Column("id_factura")]
        public int IdFactura { get; set; }

        [Column("saldo")]
        public decimal Saldo { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }


        //[ForeignKey(nameof(IdProveedor))]
        //public Pedido.Pedido? Pedido { get; set; }
    }
}
