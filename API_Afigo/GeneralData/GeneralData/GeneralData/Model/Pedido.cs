using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Newtonsoft.Json;




namespace GeneralData.Model
{

    public class Pedido
    {

        public int id_pedido { get; set; }
        public string estado { get; set; }
        public int id_usuario { get; set; }
        public string nombre_cliente { get; set; }
        public int factura_electronica { get; set; }
        public string detalle_factura { get; set; }
        public string metodo_envio { get; set; }
        public string direccion_envio { get; set; }
        public string urgencia { get; set; }
        public string tipo_pedido { get; set; }
        public string sucursal { get; set; }
        public string codigo { get; set; }

        public int usuario_admin { get; set; }
        




        public Pedido()
        {
            this.id_pedido = 0;
            this.estado = "";
            this.id_usuario = 0;
            this.nombre_cliente = "";
            this.factura_electronica = 0;
            this.detalle_factura = "";
            this.metodo_envio = "";
            this.direccion_envio = "";
            this.urgencia = "";
            this.tipo_pedido = "";
            this.sucursal = "";
            this.codigo = "";
        }

        public string AllPedidos()
        {

            Conexion conexion = new Conexion();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spPedidoAll";

            DataTable dt = conexion.ExecuteReaderDataTable(cmd);

            return JsonConvert.SerializeObject(dt);
        }

        public string PedidoPorTipo(string tipo_pedido, string sucursal, int tipo_usuario)
        {

            Conexion conexion = new Conexion();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spPedidoPorTipo";
            cmd.Parameters.AddWithValue("@tipo_pedido", tipo_pedido);
            cmd.Parameters.AddWithValue("@sucursal", sucursal);
            cmd.Parameters.AddWithValue("@tipo_usuario", tipo_usuario);

            DataTable dt = conexion.ExecuteReaderDataTable(cmd);

            return JsonConvert.SerializeObject(dt);
        }

        #region Crud de Pedidos

        public void New(string estado, int id_usuario, string nombre_cliente,
            int factura_electronica, string detalle_factura, string metodo_envio,
            string direccion_envio, string urgencia, string tipo_pedido, string sucursal,string codigo)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spPedidoNew");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@estado", estado);
            comando.Parameters.AddWithValue("@id_usuario", id_usuario);
            comando.Parameters.AddWithValue("@nombre_cliente", nombre_cliente);
            comando.Parameters.AddWithValue("@factura_electronica", factura_electronica);
            comando.Parameters.AddWithValue("@detalle_factura", detalle_factura);
            comando.Parameters.AddWithValue("@metodo_envio", metodo_envio);
            comando.Parameters.AddWithValue("@direccion_envio", direccion_envio);
            comando.Parameters.AddWithValue("@urgencia", urgencia);
            comando.Parameters.AddWithValue("@tipo_pedido", tipo_pedido);
            comando.Parameters.AddWithValue("@sucursal", sucursal);
            comando.Parameters.AddWithValue("@codigo", codigo);

            conexion.ExecuteNonQuery(comando);
        }


        public void Update(int id_pedido, string estado, string nombre_cliente,
            int factura_electronica, string detalle_factura, string metodo_envio,
            string direccion_envio, string urgencia, string tipo_pedido, string codigo)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spPedidoUpdate");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@id_pedido", id_pedido);
            comando.Parameters.AddWithValue("@estado", estado);
            comando.Parameters.AddWithValue("@nombre_cliente", nombre_cliente);
            comando.Parameters.AddWithValue("@factura_electronica", factura_electronica);
            comando.Parameters.AddWithValue("@detalle_factura", detalle_factura);
            comando.Parameters.AddWithValue("@metodo_envio", metodo_envio);
            comando.Parameters.AddWithValue("@direccion_envio", direccion_envio);
            comando.Parameters.AddWithValue("@urgencia", urgencia);
            comando.Parameters.AddWithValue("@tipo_pedido", tipo_pedido);
            comando.Parameters.AddWithValue("@codigo", codigo);

            conexion.ExecuteNonQuery(comando);
        }


        public void Delete(int id_pedido)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spPedidoDelete");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@id_pedido", id_pedido);

            conexion.ExecuteNonQuery(comando);
        }


        public string OneBySucursal(string sucursal)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spPedido_sucursal");

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@sucursal", sucursal);

            DataTable dt = conexion.ExecuteReaderDataTable(comando);

            return JsonConvert.SerializeObject(dt);
        }



        public string lastPedido(int id_usuario)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spUltimo_id_Pedido");

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@id_usuario", id_usuario);

            DataTable dt = conexion.ExecuteReaderDataTable(comando);

            return JsonConvert.SerializeObject(dt);
        }


        #endregion



    }
}