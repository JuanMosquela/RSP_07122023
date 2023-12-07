using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace Entidades.Files
{
    
using System.Text;
    public static class FileManager
    {

        private static string path;

        static FileManager()
        {
            FileManager.path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SP_07122023");
            FileManager.ValidarExistenciaDeDirectorio();
        }

        private static void ValidarExistenciaDeDirectorio()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    throw new FileManagerException("Error al intentar crear el directorio, no existe");
                }
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);        
            }
        }

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string filePath = Path.Combine(path, nombreArchivo);

                using (StreamWriter writer = new StreamWriter(filePath, append, Encoding.UTF8))
                {
                    writer.Write(data);
                }
            }
            catch (Exception ex)
            {
                throw new FileManagerException("Error al guardar el archivo", ex);
            }
        }

        public static bool Serializar<T>(T elemento, string nombreArchivo)
        {
            try
            {
                string filePath = Path.Combine(path, nombreArchivo);

                string json = JsonSerializer.Serialize(elemento);

                Guardar(json, nombreArchivo, false);

                return true;
            }
            catch (Exception ex)
            {
                FileManager.Guardar(ex.Message, "logs.txt", true);
                throw new FileManagerException("Error al serializar el elemento", ex);
            }
        }
    }

   
}

