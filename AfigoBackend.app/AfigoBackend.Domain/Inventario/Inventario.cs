using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Inventario
{
    [Table("Inventario")]
    public class Inventario
    {
        [Key]
        [Column("id_inventario")]
        public int IdInventario { get; set; }

        [Column("sucursal")]
        public string Sucursal { get; set; } = string.Empty;

        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("cantidad")]
        public decimal Cantidad { get; set; }

        [Column("fecha_ingreso")]
        public DateOnly FechaIngreso { get; set; }
    }
}
