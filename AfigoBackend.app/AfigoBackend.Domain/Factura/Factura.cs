using AfigoBackend.Domain.Cuenta;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Factura
{
    [Table("Factura")]
    public class Factura
    {
        [Key]
        [Column("id_factura")]
        public int IdFactura { get; set; }

        [Column("numero")]
        public string Numero { get; set; } = string.Empty;

        [Column("estado")]
        public decimal Estado { get; set; }

        [Column("sucursal")]
        public string Sucursal { get; set; } = string.Empty;

        [Column("fecha")]
        public DateOnly Fecha { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente.Cliente? Cliente { get; set; }
    }
}
