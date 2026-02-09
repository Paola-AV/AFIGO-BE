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
        public string PrimerNombre { get; set; } = string.Empty;

        [Column("segundo_nombre")]
        public string SegundoNombre { get; set; } = string.Empty;

        [Column("primer_apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [Column("segundo_apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [Column("cedula_fisica")]
        public string CedulaFisica { get; set; } = string.Empty;

        [Column("cedula_juridica")]
        public string CedulaJuridica { get; set; } = string.Empty;

        [Column("correo_electronico")]
        public string CorreoElectronico { get; set; } = string.Empty;
        
        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

    }
}
