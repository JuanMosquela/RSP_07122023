using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static SqlConnection connection;
        private static string stringConnection;        
        static  DataBaseManager()
        {
            DataBaseManager.stringConnection = "server=.\\SQLEXPRESS; database=20230622SP; integrated security=true";
        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = $"SELECT imagen FROM COMIDA WHERE tipo_comida = @tipo";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tipo", tipo);
                    connection.Open();                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return (string)reader["imagen"];
                    }                    
                }

            }catch (Exception ex)
            {
                throw new ComidaInvalidaExeption("Error al obtener la imagen de la comida en la base de datos");
            }
            finally
            {
                connection.Close();
            }         
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = $"INSERT INTO TICKET (empleado, ticket) VALUES (@empleado,@ticket)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@nombreEmpleado", nombreEmpleado);
                    command.Parameters.AddWithValue("@ticket", comida.Ticket);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();

                    return true;                    
                }
            }
            catch(Exception ex)
            {
                throw new ComidaInvalidaExeption("Error al guardar el ticket");
                
            }
           
        }
    }
}
