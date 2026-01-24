using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Newtonsoft.Json;




namespace GeneralData.Model { 
	public class detallePedido
	{
        public int id_detalle { get; set; }
        public int id_pedido { get; set; }
        public string nombre_producto { get; set; }
        public int cant_producto { get; set; }
        public string descripcion { get; set; }
        public string codigo { get; set; }
        public string estado { get; set; }

        public detallePedido()
		{

            this.id_detalle = 0;
            this.id_pedido = 0;
            this.nombre_producto = "";
            this.cant_producto = 0;
            this.descripcion = "";
            this.estado = "";
            this.codigo = "";
        }

        public string AllDetalles(int id_pedido)
        {

            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spDetallePedidoGet");

            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@id_pedido", id_pedido);

            DataTable dt = conexion.ExecuteReaderDataTable(comando);

            return JsonConvert.SerializeObject(dt);
        } 


        public void New(int id_pedido, string nombre_producto, int cant_producto, string descripcion, string estado,string codigo)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spDetalle_pedidoNew");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@id_pedido", id_pedido);
            comando.Parameters.AddWithValue("@nombre_producto", nombre_producto);
            comando.Parameters.AddWithValue("@cant_producto", cant_producto);
            comando.Parameters.AddWithValue("@descripcion", descripcion);
            comando.Parameters.AddWithValue("@estado", estado);
            comando.Parameters.AddWithValue("@codigo", codigo);
            conexion.ExecuteNonQuery(comando);
        }



        public void Update(int id_detalle, int id_pedido, string nombre_producto, int cant_producto, string descripcion, string estado,string codigo)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spDetalle_pedidoUpdate");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@id_detalle", id_detalle);
            comando.Parameters.AddWithValue("@id_pedido", id_pedido);
            comando.Parameters.AddWithValue("@nombre_producto", nombre_producto);
            comando.Parameters.AddWithValue("@cant_producto", cant_producto);
            comando.Parameters.AddWithValue("@decripcion", descripcion);
            comando.Parameters.AddWithValue("@estado", estado);
            comando.Parameters.AddWithValue("@codigo", codigo);
            conexion.ExecuteNonQuery(comando);
        }

        public void Delete(int id_detalle)
        {
            Conexion conexion = new Conexion();

            SqlCommand comando = new SqlCommand("spDetalle_ProductoDelete");
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@id_detalle", id_detalle);

            conexion.ExecuteNonQuery(comando);
        }


    }
}