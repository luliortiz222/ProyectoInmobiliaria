using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class InquilinoRepository
{
    private string _cadenaConexion;

    public InquilinoRepository(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    public void Guardar(Inquilino inquilino)
    {
        string query = @"INSERT INTO Inquilino (Dni, Nombre, Apellido, Email, Telefono) 
                        VALUES (@Dni, @Nombre, @Apellido, @Email, @Telefono)";

        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Dni", inquilino.Dni);
                comando.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                comando.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                comando.Parameters.AddWithValue("@Email", inquilino.Email);
                comando.Parameters.AddWithValue("@Telefono", inquilino.Telefono);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    Console.WriteLine("Inquilino guardado con éxito");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al guardar inquilino: " + ex.Message);
                }
            }
        }
    }

    public List<Inquilino> ObtenerTodos()
    {
        List<Inquilino> lista = new List<Inquilino>();
        string query = "SELECT * FROM Inquilino";

        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                try
                {
                    conexion.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Inquilino i = new Inquilino
                            {
                                IdInquilino = Convert.ToInt32(reader["IdInquilino"]),
                                Dni = reader["Dni"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString()
                            };
                            lista.Add(i);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener datos: " + ex.Message);
                }
            }
        }
        return lista;
    }
}