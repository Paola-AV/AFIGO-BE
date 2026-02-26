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
        public string? Numero { get; set; } = string.Empty;

        [Column("estado")]
        public string? Estado { get; set; } = string.Empty;

        [Column("sucursal")]
        public string? Sucursal { get; set; } = string.Empty;

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("id_proveedor")]
        public int? IdProveedor { get; set; }

        [Column("identificadorExt")]
        public int? IdentificadorExt { get; set; }

    }
}
