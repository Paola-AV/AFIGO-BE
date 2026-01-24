using GeneralData.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace GeneralData.Controllers
{
    [Route("api/pedido")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class pedidoController : ControllerBase
    {
        [HttpGet, Route("All")]
        [Authorize]
        public string AllPedidos()
        {
            Pedido myPedido = new Pedido();
            return myPedido.AllPedidos();
        }

        [HttpPost, Route("ByTypePedido")]
        [Authorize]
        public string PedidoPorTipoPedido([FromBody] Pedido myPedido)
        {
            return myPedido.PedidoPorTipo("Pedido", myPedido.sucursal, myPedido.usuario_admin);
        }

        [HttpPost, Route("ByTypeCotizacion")]
        [Authorize]
        public string PedidoPorTipoCotizacion([FromBody] Pedido myPedido)
        {
            return myPedido.PedidoPorTipo("Cotizacion", myPedido.sucursal, myPedido.usuario_admin);
        }

        [HttpPost, Route("ByTypeProveedor")]
        [Authorize]
        public string PedidoPorTipoProveedor([FromBody] Pedido myPedido)
        {
            return myPedido.PedidoPorTipo("Proveedor", myPedido.sucursal, myPedido.usuario_admin);
        }


        [HttpPost, Route("bySucursal")]
        [Authorize]
        public string bySucursal(string sucursal)
        {
            Pedido myPedido = new Pedido();
            return myPedido.OneBySucursal(sucursal); 
        }

        [HttpPost, Route("create")]
        [Authorize]
        public  async Task<IActionResult> createPedido([FromBody] Model.Pedido pedido)
        {
            try
            { 
               Pedido myPedido = new Pedido();
                pedido.New(pedido.estado, pedido.id_usuario, pedido.nombre_cliente, pedido.factura_electronica, pedido.detalle_factura, pedido.metodo_envio, pedido.direccion_envio, pedido.urgencia, pedido.tipo_pedido, pedido.sucursal,pedido.codigo);
                string myIdPedido = myPedido.lastPedido(pedido.id_usuario).Replace("[{\"id_pedido\":", "").Replace("}]", "");
                pedido.id_pedido = int.Parse(myIdPedido);
                return   Ok(new { Respuesta = "Se ha creado con exito el pedido.", pedido });                   
            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }
         


        [HttpPut, Route("update")]
        [Authorize]
        public IActionResult updatePedido([FromBody] Model.Pedido pedido)
        {
            try
            {

                pedido.Update(pedido.id_pedido, pedido.estado, pedido.nombre_cliente, pedido.factura_electronica, pedido.detalle_factura, pedido.metodo_envio, pedido.direccion_envio, pedido.urgencia, pedido.tipo_pedido,pedido.codigo);
                return Ok(new { Respuesta = "Se ha creado con exito el pedido.", pedido });

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }



        [HttpDelete, Route("delete")]
        [Authorize]
        public IActionResult deletePedido([FromBody] Model.Pedido pedido)
        {
            try
            {
                pedido.Delete(pedido.id_pedido);
                return Ok(new { Respuesta = "Se ha eliminado el pedido correctamente" });

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }

    }
}
