using System;
// librería de MySQL
using MySql.Data.MySqlClient;

namespace ProyectoInmobiliaria

{
    class Program
    {
        static void Main(string[] args)
        {
            
            string cadenaConexion = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=admin;";

            
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    
                    conexion.Open();
                    Console.WriteLine("¡Conexión a MySQL establecida con éxito!");

                    

                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("Error al conectar: " + ex.Message);
                }
            }
        }
    }
}