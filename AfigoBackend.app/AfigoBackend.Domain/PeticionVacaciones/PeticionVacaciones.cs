using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.PeticionVacaciones
{
    [Table("PeticionVacaciones")]
    public class PeticionVacaciones
    {
        [Key]
        [Column("id_peticion")]
        public int IdPeticion { get; set; }

        [Column("id_trabajador")]
        public int IdTrabajador { get; set; }

        [Column("fecha_inicio")]
        public DateOnly FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateOnly FechaFin { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = string.Empty;
    }
}
