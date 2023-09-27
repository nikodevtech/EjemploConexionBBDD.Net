using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql; //Necesario instalar NPGSQL con el administrador de paquetes para trabajar con BBDD 
using System.Data;
using System.Configuration;

namespace EjemploConexionBBDD.Net
{
    /// <summary>
    /// Ejemplo básico de conexión a una base de datos postgresql en .net y realizar una consulta
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {

            // Cadena de conexión a la bbdd usando los datos asociados en el archivo app.config con ayuda del ConfigurationManager
            string urlConexion = ConfigurationManager.ConnectionStrings["MiConexionBD"].ConnectionString;


            //Instancia conexion a la BBDD para la que se establecerá utilizando los datos de la url antes creada.
            //Using asegura que los recursos desechables en este bloque, como conexiones de base de datos, se cierren y liberen
            using (NpgsqlConnection conexion = new NpgsqlConnection(urlConexion))
            {
                try
                {
                    conexion.Open(); //Con dicho metodoAbre la conexión 
                    Console.WriteLine("\n\tConexión exitosa a PostgreSQL.");
                    string query = "SELECT * FROM gbp_almacen.gbp_alm_cat_libros"; //Consulta a nuestra tabla de la BBDD

                    //Objeto comando que se utilizará para enviar consultas SQL a la base de datos 
                    //pasandole por argumento la misma query y el objeto conexión
                    using (NpgsqlCommand comando = new NpgsqlCommand(query, conexion))
                    {
                        //El objeto lector utilizará para leer y procesar los resultados que devuelva la query a la BBDD
                        using (NpgsqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read()) //Nos permite iterar para leer el resultado de la query
                            {
                                Console.WriteLine(
                                    String.Format("\n\n\tid_libro: {0}\n\tTitulo: {1}\n\tAutor: {2}\n\tISBN: {3}\n\tEdición: {4}"
                                    , lector["id_libro"].ToString() //Accediendo a cada posición de la estructura para mostrar
                                    , lector["titulo"].ToString()
                                    , lector["Autor"].ToString()
                                    , lector["isbn"].ToString()
                                    , lector["edicion"].ToString()
                                    ));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\tError al conectar a PostgreSQL: " + e.Message);
                }
            }
            Console.WriteLine("\n\tPulse una tecla para salir...");
            Console.ReadKey();
        }
    }
}






