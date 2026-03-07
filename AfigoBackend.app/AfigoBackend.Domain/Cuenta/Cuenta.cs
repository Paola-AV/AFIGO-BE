
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AfigoBackend.Domain.Cuenta
{
    [Table("Cuenta")]
    public class Cuenta
    {
        [Key]
        [Column("id_cuenta")]
        public int IdCuenta { get; set; }

        [Column("id_proveedor")]
        public int? IdProveedor { get; set; }

        [Column("monto")]
        public double? Monto { get; set; }

        [Column("id_factura")]
        public int? IdFactura { get; set; }

        [Column("saldo")]
        public double? Saldo { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

    }
}
