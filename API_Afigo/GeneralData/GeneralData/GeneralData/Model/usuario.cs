using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Newtonsoft.Json;

namespace GeneralData.Model
{
    public class usuario
    {
        public int user_id { get; set; }
        public string direccion { get; set; }
        public string nombre { get; set; }
        public int usuario_admin { get; set; }
        public string nombre_de_usuario { get; set; }
        public string contrasenia { get; set; }
        public string sucursal { get; set; }

        public usuario()
        {
            this.user_id = 0; 
            this.direccion = "";
            this.nombre = "";
            this.usuario_admin = 0;
            this.nombre_de_usuario = "";
            this.contrasenia = "";
            this.sucursal = "";

        }

        public List<Model.usuario> Login()
        {
            Conexion conexion = new Conexion();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spLogin";//Procedimiento almacenado
            cmd.Parameters.AddWithValue("@user", nombre_de_usuario);
            cmd.Parameters.AddWithValue("@contrasenna", contrasenia);

            DataSet ds = conexion.ExecuteReader(cmd);

            List<Model.usuario> list = new List<usuario>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                usuario p = new usuario();

                p.user_id = Convert.ToInt32(row["user_id"]);
                p.direccion = row["direccion"].ToString();
                p.nombre = row["nombre"].ToString();
                p.usuario_admin = Convert.ToInt32(row["usuario_admin"]);
                p.nombre_de_usuario = row["nombre_de_usuario"].ToString();
                p.sucursal = row["sucursal"].ToString();


                list.Add(p);
            }
            return list;
        }

        public string AllUser()
        {

            Conexion conexion = new Conexion();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUserAll";

            DataTable dt = conexion.ExecuteReaderDataTable(cmd);

            return JsonConvert.SerializeObject(dt);
        }

        // Crud
        #region Crud de usuarios
        public void New(string nombre, string direccion, int usuario_admin, string nombre_de_usuario, string contrasenia,string sucursal)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spUserNew");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@direccion", direccion);
            comando.Parameters.AddWithValue("@usuario_admin", usuario_admin);
            comando.Parameters.AddWithValue("@nombre_de_usuario", nombre_de_usuario);
            comando.Parameters.AddWithValue("@contrasenia", contrasenia);
            comando.Parameters.AddWithValue("@sucursal", sucursal);


            conexion.ExecuteNonQuery(comando);
        }

        public void Update(int user_id, string nombre, string direccion, int usuario_admin, string nombre_de_usuario, string contrasenia, string sucursal)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spplatform_userUpdate");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@user_id", user_id);
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@direccion", direccion);
            comando.Parameters.AddWithValue("@usuario_admin", usuario_admin);
            comando.Parameters.AddWithValue("@nombre_de_usuario", nombre_de_usuario);
            comando.Parameters.AddWithValue("@contrasenia", contrasenia);
            comando.Parameters.AddWithValue("@sucursal", sucursal);

            conexion.ExecuteNonQuery(comando);
        } 

        public string One(int user_id)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spplatform_userOne");

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@user_id", user_id);

            DataTable dt = conexion.ExecuteReaderDataTable(comando);

            return JsonConvert.SerializeObject(dt);
        }

        public string OneByName(string nombre_de_usuario)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spplatform_userNombreUsuario");

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@nombre_de_usuario", nombre_de_usuario);

            DataTable dt = conexion.ExecuteReaderDataTable(comando);

            return JsonConvert.SerializeObject(dt);
        }

        public void Delete(int user_id)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spplatform_userDelete");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@user_id", user_id);

            conexion.ExecuteNonQuery(comando);
        }
        #endregion
    }
}
