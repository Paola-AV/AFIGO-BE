using System.ComponentModel.DataAnnotations;

namespace AfigoControl.Models
{
    public class Pedido
    {
        [Key]

        public int id_pedido { get; set; }
        public DateTime fecha_pedido { get; set; }
        public string estado { get; set; }
        public int id_usuario { get; set; }
        public string nombre_cliente { get; set; }
        public int factura_electronica { get; set; }
        public string detalle_factura { get; set; }
        public string metodo_envio { get; set; }
        public string direccion_envio { get; set; }
        public string urgencia_encio { get; set; }
        public string tipo_pedido { get; set; }


    }
}
