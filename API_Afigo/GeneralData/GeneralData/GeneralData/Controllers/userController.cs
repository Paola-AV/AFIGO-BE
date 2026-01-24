using GeneralData.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http.Cors;

namespace GeneralData.Controllers
{
    [Route("api/user")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class userController : ControllerBase
    {
        [HttpGet, Route("All")]
        [Authorize]
        public string AllUser()
        {
            usuario myUser = new usuario();
            return myUser.AllUser();
        }

        [HttpGet, Route("One")]
        [Authorize]
        public string OneUser([FromBody] Model.usuario Usuario)
        {
            return Usuario.One(Usuario.user_id);
        }

        [HttpPost, Route("create")]
        [Authorize]
        public IActionResult createUser([FromBody] Model.usuario Usuario)
        {
            try
            {
                if (Usuario.contrasenia == "" || Usuario.contrasenia == null ||
                    Usuario.nombre_de_usuario == "" || Usuario.nombre_de_usuario == null)
                {
                    return Ok(new { Respuesta = "No se pueden agregar usuarios sin contraseña o algún nombre de usuario" });
                }
                else 
                {
                    if (Usuario.OneByName(Usuario.nombre_de_usuario) != "[]")//Si encuentra un usuario con el mismo nombre
                    {
                        return Ok(new { Respuesta = "El usuario ya existe" });
                    }
                    else
                    {
                        Usuario.New(Usuario.nombre, Usuario.direccion, Usuario.usuario_admin, Usuario.nombre_de_usuario, Usuario.contrasenia, Usuario.sucursal);
                        return Ok(new { Respuesta = "Se ha creado con exito el usuario.", Usuario });
                    }
                }

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }

        [HttpPut, Route("update")]
        [Authorize]
        public IActionResult updateUser([FromBody] Model.usuario Usuario)
        {
            try
            {
                if (Usuario.contrasenia == "" || Usuario.contrasenia == null ||
                    Usuario.nombre_de_usuario == "" || Usuario.nombre_de_usuario == null)
                {
                    return Ok(new { Respuesta = "No se pueden actualizar usuarios sin contraseña o algún nombre de usuario" });
                }
                else
                {
                    string Json = Usuario.One(Usuario.user_id).Replace("[", "").Replace("]", "");
                    if (Json != "")
                    {
                        Usuario.Update(Usuario.user_id, Usuario.nombre, Usuario.direccion, Usuario.usuario_admin, Usuario.nombre_de_usuario, Usuario.contrasenia, Usuario.sucursal);
                        return Ok(new { Respuesta = "Se ha actualizado con exito el usuario.", Usuario });
                    }
                    else {
                        return Ok(new { Respuesta = "Error al ejecutar el endpoint" });

                    } 
                }

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }

        [HttpDelete, Route("delete")]
        [Authorize]
        public IActionResult deleteUser([FromBody] Model.usuario Usuario)
        {
            try
            {
                Usuario.Delete(Usuario.user_id);
                return Ok(new { Respuesta = "Se ha eliminado el usuario correctamente" });

            }
            catch (Exception)
            {
                return Ok(new { Respuesta = "Error al ejecutar el endpoint" });
            }
        }
    }
}
