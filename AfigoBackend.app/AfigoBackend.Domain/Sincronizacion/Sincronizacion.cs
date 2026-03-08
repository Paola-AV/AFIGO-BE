using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Sincronizacion
{
    public class Sincronizacion
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public DateTime UltimaFecha { get; set; }
    }
}
