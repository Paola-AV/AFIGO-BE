using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Usuario
{
    [Table("Usuario")]
    public class Usuario
    {

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("nombre")]
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("usuario_admin")]
        public int UsuarioAdmin { get; set; }

        [Column("nombre_de_usuario")]
        [Required]
        public string NombreDeUsuario { get; set; } = string.Empty;

        [Column("contrasenia")]
        [Required]
        public string Contrasenia { get; set; } = string.Empty;

    }
}
