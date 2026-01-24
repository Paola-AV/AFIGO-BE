using GeneralData.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http.Cors;

namespace GeneralData.Controllers
{
    [Route("api/detalle")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]


    public class detalleController : ControllerBase
    {
        [HttpPost, Route("All")]
        [Authorize]
        public string AllDetalles([FromBody] int id_pedido)
        {
            detallePedido mydetalle = new detallePedido();
            return mydetalle.AllDetalles(id_pedido);
        }

         

        [HttpPost, Route("create")]
        [Authorize]
        public IActionResult createDetalle([FromBody] Model.detallePedido detalle)
        {
            try
            {
                detalle.New(detalle.id_pedido, detalle.nombre_producto, detalle.cant_producto, detalle.descripcion, detalle.estado);
                return Ok(new { Respuesta = "Se ha creado con exito el detalle del pedido.", detalle });
            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }


        [HttpPut, Route("update")]
        [Authorize]
        public IActionResult updateDetalle([FromBody] Model.detallePedido detalle)
        {
            try
            {

                detalle.Update(detalle.id_detalle, detalle.id_pedido, detalle.nombre_producto, detalle.cant_producto, detalle.descripcion, detalle.estado);
                return Ok(new { Respuesta = "Se ha creado con exito el pedido.", detalle });

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }


        [HttpDelete, Route("delete")]
        [Authorize]
        public IActionResult deleteDetalle([FromBody] Model.detallePedido detalle)
        {
            try
            {
                detalle.Delete(detalle.id_detalle);
                return Ok(new { Respuesta = "Se ha eliminado el detalle de pedido correctamente" });

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }



    }
}