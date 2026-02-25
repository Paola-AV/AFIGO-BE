using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Domain.Proveedor
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Column("primer_nombre")]
        public string? PrimerNombre { get; set; }

        [Column("segundo_nombre")]
        public string? SegundoNombre { get; set; }

        [Column("primer_apellido")]
        public string? PrimerApellido { get; set; } 
        [Column("segundo_apellido")]
        public string? SegundoApellido { get; set; } 

        [Column("cedula_fisica")]
        public string? CedulaFisica { get; set; } 

        [Column("cedula_juridica")]
        public string? CedulaJuridica { get; set; } 

        [Column("correo_electronico")]
        public string? CorreoElectronico { get; set; }
        
        [Column("direccion")]
        public string? Direccion { get; set; } 

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("identificadorExt")]
        public int? IdentificadorExt { get; set; }

    }
}
