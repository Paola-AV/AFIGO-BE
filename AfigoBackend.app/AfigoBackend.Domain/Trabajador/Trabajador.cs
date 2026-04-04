using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Trabajador
{
    [Table("Trabajador")]
    public class Trabajador
    {
        [Key]
        [Column("id_trabajador")]
        public int IdTrabajador { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("fecha_inicio")]
        public DateOnly FechaInicio { get; set; }

        [Column("vacaciones_disponibles")]
        public decimal VacacionesDisponibles { get; set; }

        [Column("vendedor")]
        public int? Vendedor { get; set; }

        [Column("nombre_vendedor")]
        public string? NombreVendedor { get; set; }

        [Column("sede")]
        public string? Sede { get; set; }
    }
}
