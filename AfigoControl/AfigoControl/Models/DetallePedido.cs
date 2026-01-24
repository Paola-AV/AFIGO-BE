using System.ComponentModel.DataAnnotations;

namespace AfigoControl.Models
{
    public class DetallePedido
    {
        [Key]

        public int id_detalle { get; set; }
        public int id_pedido { get; set; }
        public string nombre_producto { get; set; }
        public int cant_producto { get; set; }
        public string descripcion { get; set; }
      


    }
}
