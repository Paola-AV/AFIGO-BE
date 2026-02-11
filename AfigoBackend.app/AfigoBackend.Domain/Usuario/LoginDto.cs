using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Usuario
{
    public class LoginDto
    {
        public string CorreoOUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
