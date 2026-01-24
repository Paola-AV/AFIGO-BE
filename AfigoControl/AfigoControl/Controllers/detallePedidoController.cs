using AfigoControl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AfigoControl.Controllers
{
    public class detallePedidoController : ControllerBase
    {


        private readonly UserDbContext _userDbContext;

        public detallePedidoController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("GetDetalle")]

        public async Task<IEnumerable<DetallePedido>> GetDetalle()
        {
            return await _userDbContext.DetallePedidos.ToListAsync();

        }


        [HttpPost]
        [Route("AddDetalle")]

        public async Task<DetallePedido> AddDetalle(DetallePedido objDetalle)
        {
            _userDbContext.DetallePedidos.Add(objDetalle);
            await _userDbContext.SaveChangesAsync();
            return objDetalle;
        }


        [HttpPatch]
        [Route("UpdateUDetalle/{id}")]

        public async Task<DetallePedido> UpdateDetalle(DetallePedido objDetalle)
        {
            await _userDbContext.SaveChangesAsync();
            return objDetalle;
        }

        [HttpDelete]
        [Route("DeleteDetalle/{id}")]

        public bool DeleteDetalle(int id)
        {
            bool a = false;
            var detalle_pedido = _userDbContext.DetallePedidos.Find(id);

            if (detalle_pedido != null)
            {
                a = true;
                _userDbContext.Entry(detalle_pedido).State = EntityState.Modified;
                _userDbContext.SaveChanges();
            }
            else
            {

                a = false;

            }

            return a;


        }
    }
}
