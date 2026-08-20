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

                    PropietarioRepository repo = new PropietarioRepository(cadenaConexion);
                    //Propietario propietario = new Propietario
                    //{
                    //    Nombre = "Juan",
                    //    Apellido = "Pérez",
                    //    Dni = "12345678",
                    //    Email = "juanCaballo@example.com",
                    //    Telefono = "555-1234"
                    //};
                    //repo.guardar(propietario);

                    //foreach (Propietario p in repo.ObtenerTodos())
                    //{
                    //    Console.WriteLine($"Nombre: {p.Nombre}, Apellido: {p.Apellido}, DNI: {p.Dni}, Email: {p.Email}, Teléfono: {p.Telefono}");
                    //}

                    InquilinoRepository repoIn = new InquilinoRepository(cadenaConexion);

                    //Inquilino inquilino = new Inquilino
                    //{
                    //    Nombre = "María",
                    //    Apellido = "Gómez",
                    //    Dni = "87654321",
                    //    Email = "Mgomez@example.com",
                    //    Telefono = "555-5678"
                    //};
                    //repoIn.Guardar(inquilino);


                    Console.WriteLine("Propietario: Dni: "+ repo.obtenerPorDni("12345678").Dni + ", Nombre: " + repo.obtenerPorDni("12345678").Nombre + ", Apellido: " + repo.obtenerPorDni("12345678").Apellido + ", Email: " + repo.obtenerPorDni("12345678").Email + ", Teléfono: " + repo.obtenerPorDni("12345678").Telefono );

                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error al conectar: " + ex.Message);
                }
            }
        }
    }
}