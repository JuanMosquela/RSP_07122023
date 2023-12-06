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
                    string query = $"SELECT imagen FROM COMIDA WHERE tipo_comida = {tipo}";
                    SqlCommand command = new SqlCommand(query, connection);
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
        }

        public static bool GuardarTicket<T>(string nombreEmpleado, T comida)
        {
            return false;
        }
    }
}
