using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.ExternalViews
{

    [Keyless]
    public class ExternalProductoView
    {
        public string? IdProducto { get; set; }     // varchar(20)
        public string? Nombre { get; set; }         // varchar(100)
        public string? Descripcion { get; set; }    // varchar(20)
        public double? PrecioCosto { get; set; }    // float
        public double? PrecioVenta { get; set; }    // float
        public string? Familia { get; set; }        // varchar(50)
        public string? Marca { get; set; }          // varchar(50)
    }

    [Keyless]
    public class ExternalGastoView
    {
        public string Tipo { get; set; } = string.Empty;      // varchar(200)
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }                   // smalldatetime
        public double Monto { get; set; }                     // float
        public string Sucursal { get; set; } = string.Empty;  // varchar(14)
    }

    [Keyless]
    public class ExternalInventarioView
    {
        public string Sucursal { get; set; } = string.Empty;  // varchar(14)
        public string IdProducto { get; set; } = string.Empty; // varchar(20)
        public double? Cantidad { get; set; }                  // float
        public DateTime FechaIngreso { get; set; }            // datetime
    }
}
