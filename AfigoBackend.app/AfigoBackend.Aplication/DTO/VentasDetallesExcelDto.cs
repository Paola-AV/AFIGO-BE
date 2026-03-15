using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.DTO
{
    public class VentasDetallesExcelDto
    {
        [Display(Name = "N° Factura", Order = 1)]
        public string? NumFactura { get; set; }

        [Display(Name = "Fecha", Order = 2)]
        [DisplayFormat(DataFormatString = "yyyy-mm-dd")]
        public DateTime? Fecha { get; set; }

        [Display(Name = "Descripción", Order = 3)]
        public string? Descripcion { get; set; }

        [Display(Name = "Estado", Order = 4)]
        public string? Estado { get; set; }

        [Display(Name = "Vendedor", Order = 5)]
        public string? NombreVendedor { get; set; }

        [Display(Name = "Cliente", Order = 6)]
        public string? NombreCliente { get; set; }

        [Display(Name = "Referencia", Order = 7)]
        public string? Referencia { get; set; }

        [Display(Name = "Monto Total", Order = 8)]
        [DisplayFormat(DataFormatString = "#,##0.00")]
        public double? MontoTotal { get; set; }

        [Display(Name = "Producto", Order = 9)]
        public string? NombreProducto { get; set; }

        [Display(Name = "Familia", Order = 10)]
        public string? FamiliaProducto { get; set; }

        [Display(Name = "Cantidad", Order = 11)]
        public double? Cantidad { get; set; }
    }
}
