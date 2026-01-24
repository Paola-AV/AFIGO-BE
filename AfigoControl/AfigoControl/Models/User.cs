using System.ComponentModel.DataAnnotations;


namespace AfigoControl.Models
{
    public class User
    {
        [Key]

        public int user_id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public int usuario_admin { get; set; }
        public string nombre_de_usuario { get; set; }
        public string contrasenia { get; set; }



    }
}
