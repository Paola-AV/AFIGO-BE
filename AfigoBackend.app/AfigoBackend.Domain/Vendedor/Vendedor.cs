using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AfigoBackend.Domain.Vendedor
{
    [Table("Vendedor")]
    public class Vendedor
    {
        [Key]
        [Column("id_vendedor")]
        public int IdVendedor { get; set; }

        [Column("id_bodega")]
        public int IdBodega { get;set; }

        [Column("id_vendedorExt")]
        public int IdVendedorExt { get;set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

    }
}
