using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralData.Model
{
    public class Conexion
    {
        private string server = "Afigo_control.mssql.somee.com";
        private string db = "Afigo_control";
        private string user = "compartidoDev_SQLLogin_1";
        private string pass = "47489168ny";

        public Conexion()
        {
        }

        public DataSet ExecuteReader(SqlCommand cmd)
        {

            DataSet ds = new DataSet();

            SqlConnection conn = new SqlConnection();
            cmd.Connection = conn;
            conn.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pass + "; Connection Timeout=200; pooling=true;Max Pool Size=32767;MultipleActiveResultSets=True";

            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds, "tabla");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public DataSet ExecuteReader(SqlCommand cmd, bool email = true)
        {

            DataSet ds = new DataSet();

            SqlConnection conn = new SqlConnection();
            cmd.Connection = conn;
            conn.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pass + "; Connection Timeout=200; pooling=true;Max Pool Size=32767;MultipleActiveResultSets=True";

            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds, "tabla");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public int ExecuteNonQuery(SqlCommand cmd, string nombreParametro)
        {

            int respuesta = 0;

            SqlConnection conn = new SqlConnection();
            cmd.Connection = conn;
            conn.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pass + "; Connection Timeout=200; pooling=true;Max Pool Size=32767;MultipleActiveResultSets=True";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                respuesta = Convert.ToInt32(cmd.Parameters[nombreParametro].Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }

            return respuesta;
        }

        public void ExecuteNonQuery(SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection();
            cmd.Connection = conn;
            conn.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pass + "; Connection Timeout=200; pooling=true;Max Pool Size=32767;MultipleActiveResultSets=True";

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                conn.Close();
            }

        }

        public DataTable ExecuteReaderDataTable(SqlCommand cmd)
        {

            DataTable dsTabla = new DataTable();

            SqlConnection conn = new SqlConnection();
            cmd.Connection = conn;
            conn.ConnectionString = "Data Source=" + server + ";Initial Catalog=" + db + ";User ID=" + user + ";Password=" + pass + "; Connection Timeout=200; pooling=true;Max Pool Size=32767;MultipleActiveResultSets=True";

            try
            {
                using (SqlDataAdapter adaptador = new SqlDataAdapter(cmd))
                {
                    dsTabla = new DataTable();
                    adaptador.Fill(dsTabla);
                }
                return dsTabla;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsTabla.Dispose();
                cmd.Dispose();
                conn.Close();
            }

        }
    }
}
