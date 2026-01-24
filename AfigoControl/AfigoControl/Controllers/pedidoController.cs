
using AfigoControl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AfigoControl.Controllers
{
    public class pedidoController : ControllerBase
    {

        private readonly UserDbContext _userDbContext;

        public pedidoController(UserDbContext userDbContext)
        {

            _userDbContext = userDbContext;
        }

        [HttpGet]
        [Route("GetPedido")]

        public async Task<IEnumerable<Pedido>> GetPedido()
        {
            return await _userDbContext.Pedidos.ToListAsync();

        }


        [HttpPost]
        [Route("AddPedido")]

        public async Task<Pedido> AddPedido(Pedido objPedido)
        {
            _userDbContext.Pedidos.Add(objPedido);
            await _userDbContext.SaveChangesAsync();
            return objPedido;
        }


        [HttpPatch]
        [Route("UpdatePedido/{id}")]

        public async Task<Pedido> UpdatePedido(Pedido objPedido)
        {
            await _userDbContext.SaveChangesAsync();
            return objPedido;
        }

        [HttpDelete]
        [Route("DeletePedido/{id}")]

        public bool DeletePedido(int id)
        {
            bool a = false;
            var pedido = _userDbContext.Pedidos.Find(id);

            if (pedido != null)
            {
                a = true;
                _userDbContext.Entry(pedido).State = EntityState.Modified;
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
