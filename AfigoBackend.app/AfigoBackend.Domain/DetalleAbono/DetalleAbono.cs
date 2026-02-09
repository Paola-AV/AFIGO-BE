using AfigoBackend.Domain.Pedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.DetalleAbono
{
    [Table("DetalleAbono")]
    public class DetalleAbono
    {
        [Key]
        [Column("id_detalle_abono")]
        public int IdDetalleAbono { get; set; }
        
        [Column("id_cuenta")]
        public int IdCuenta { get; set; }

        [Column("num_pago")]
        public string NumPago { get; set; } = string.Empty;

        [Column("fecha")]
        public DateOnly FechaAbono { get; set; }

        [Column("monto")]
        public decimal MontoAbono { get; set; }

        [Column("saldo")]
        public decimal Saldo { get; set; }

        [Column("observacion")]
        public string observacion { get; set; } = string.Empty;

        [ForeignKey(nameof(IdCuenta))]
        public Cuenta.Cuenta? Cuenta { get; set; }
    }
}
