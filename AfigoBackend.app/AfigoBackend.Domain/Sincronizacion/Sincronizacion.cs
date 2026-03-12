using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Sincronizacion
{
    [Table("Sincronizacion")]
    public class Sincronizacion
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public DateTime UltimaFecha { get; set; }
    }
}
