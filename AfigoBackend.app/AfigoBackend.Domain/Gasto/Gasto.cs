using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Gasto
{

    [Table("Gasto")]
    public class Gasto
    {
        [Key]
        [Column("id_gasto")]
        public int IdGasto { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("monto")]
        public decimal Monto { get; set; }

        [Column("fecha")]
        public DateOnly Fecha { get; set; }

        [Column("sucursal")]
        public string Sucursal { get; set; } = string.Empty;
    }
}
