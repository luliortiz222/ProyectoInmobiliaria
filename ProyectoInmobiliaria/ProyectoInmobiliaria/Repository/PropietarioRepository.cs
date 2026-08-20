using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;



public class PropietarioRepository
{
	private string _cadenaConexion;
	public PropietarioRepository(string cadenaConexion)
	{
		_cadenaConexion = cadenaConexion;
	}


	public void Guardar(Propietario propietario)
	{
		string query = @"INSERT INTO Propietario (Dni, Nombre, Apellido, Email, Telefono) VALUES (@Dni, @Nombre, @Apellido, @Email, @Telefono)";


		using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
		{
			using (MySqlCommand comando = new MySqlCommand(query, conexion))
			{
				comando.Parameters.AddWithValue("@Dni", propietario.Dni);
				comando.Parameters.AddWithValue("@Nombre", propietario.Nombre);
				comando.Parameters.AddWithValue("@Apellido", propietario.Apellido);
				comando.Parameters.AddWithValue("@Email", propietario.Email);
				comando.Parameters.AddWithValue("@Telefono", propietario.Telefono);

				try
				{
					conexion.Open();
					comando.ExecuteNonQuery();
					Console.WriteLine("Propietario guardado con exito");

				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}
		}

	}


    public List<Propietario> ObtenerTodos()
    {
        List<Propietario> lista = new List<Propietario>();
        string query = "SELECT * FROM Propietario";

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
                            Propietario p = new Propietario
                            {
                                IdPropietario = Convert.ToInt32(reader["IdPropietario"]),
                                Dni = reader["Dni"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString()
                            };
                            lista.Add(p);
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

