using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Util
{

    public static class Constants
    {
        public static class TiposDocumento
        {
            public static readonly string Pedido = "PEDIDO";
            public static readonly string Cotizacion = "COTIZACION";
        }

        public static class TiposSync
        {
            public static readonly string Cuentas = "CUENTAS";
            public static readonly string Inventarios = "INVENTARIOS";
            public static readonly string Gastos = "GASTOS";
            public static readonly string Facturas = "FACTURAS";
            public static readonly string Ventas = "VENTAS";
            public static readonly string Productos = "PRODUCTOS";
            public static readonly string VentaDetalles = "VENTA DETALLES";
            public static readonly string Proveedores = "PROVEEDORES";
            public static readonly string Clientes = "CLIENTES";
            public static readonly string Vendedores = "VENDEDORES";
        }
    }

}
