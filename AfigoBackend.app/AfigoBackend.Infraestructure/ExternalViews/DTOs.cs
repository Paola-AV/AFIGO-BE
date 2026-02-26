using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.ExternalViews
{

    [Keyless]
    public class ExternalProductoView
    {
        [Column("id_producto")]
        public string? IdProducto { get; set; }     // varchar(20)
        [Column("nombre")]
        public string? Nombre { get; set; }         // varchar(100)
        [Column("descripcion")]
        public string? Descripcion { get; set; }    // varchar(20)
        [Column("precio_costo")]
        public double? PrecioCosto { get; set; }    // float
        [Column("precio_venta")]
        public double? PrecioVenta { get; set; }    // float
        [Column("familia")]
        public string? Familia { get; set; }        // varchar(50)
        [Column("marca")]
        public string? Marca { get; set; }          // varchar(50)
    }

    [Keyless]
    public class ExternalGastoView
    {
        [Column("tipo")]
        public string Tipo { get; set; } = string.Empty;      // varchar(200)
        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;
        [Column("fecha")]
        public DateTime Fecha { get; set; }                   // smalldatetime
        [Column("monto")]
        public double Monto { get; set; }                     // float
        [Column("sucursal")]
        public string Sucursal { get; set; } = string.Empty;  // varchar(14)
    }

    [Keyless]
    public class ExternalInventarioView
    {
        [Column("sucursal")]
        public string? Sucursal { get; set; }   // varchar(14)
        [Column("id_producto")]
        public string? IdProducto { get; set; } = string.Empty; // varchar(20)
        [Column("cantidad")]
        public double? Cantidad { get; set; }                  // float
        [Column("fecha_ingreso")]
        public DateTime? FechaIngreso { get; set; }            // datetime
    }

    [Keyless]
    public class ExternalCuentaView
    {
        [Column("sucursal")]
        public string? Sucursal { get; set; }
        [Column("id_proveedor")]
        public int idProveedor { get; set; }
        [Column("monto")]
        public double monto { get; set; }
        [Column("id_factura")]
        public string? idFactura { get; set; }
        [Column("estado")]
        public string? estado { get; set; }
        [Column("saldo")]
        public double saldo { get; set; }
    }

    [Keyless]
    public class ExternalFacturaView
    {
        [Column("id_factura")]
        public int? IdFactura { get; set; }
        [Column("numero")]
        public string? Numero { get; set; }
        [Column("estado")]
        public string? Estado { get; set; }
        [Column("sucursal")]
        public string Sucursal { get; set; } = string.Empty;
        [Column("fecha")]
        public DateTime? Fecha { get; set; }
        [Column("id_proveedor")]
        public string? IdProveedor { get; set; }
    }
    [Keyless]
    public class ExternalProveedorView
    {
        [Column("id_proveedor")]
        public int? IdProveedor { get; set; }

        [Column("primer_nombre")]
        public string? PrimerNombre { get; set; }

        [Column("segundo_nombre")]
        public string SegundoNombre { get; set; } = string.Empty;

        [Column("primer_apeliido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Column("segundo_apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [Column("cedula_fisica")]
        public string CedulaFisica { get; set; } = string.Empty;

        [Column("cedula_juridica")]
        public string? CedulaJuridica { get; set; }

        [Column("corrreo_electronico")]
        public string? CorreoElectronico { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }
    }

    [Keyless]
    public class ExternalVentaView
    {
        [Column("id_Venta")]
        public int? IdVenta { get; set; }

        [Column("fecha")]
        public DateTime? Fecha { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("id_trabajador")]
        public int? IdTrabajador { get; set; }

        [Column("id_cliente")]
        public int? IdCliente { get; set; }

        [Column("num_factura")]
        public string? NumFactura { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("montoTotal")]
        public double? MontoTotal { get; set; }

        [Column("referencia")]
        public string? Referencia { get; set; }
    }

    [Keyless]
    public class ExternalVentaDetalleView
    {
        [Column("id_venta")]
        public int? IdVenta { get; set; }

        [Column("id_producto")]
        public string? IdProducto { get; set; }

        [Column("cantidad")]
        public double? cantidad { get; set; }
    }
}
