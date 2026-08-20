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


	public void guardar(Propietario propietario)
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


    public List<Propietario> obtenerTodos()
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
    public Propietario obtenerPorId(int id)
    {
        Propietario propietario = null;
        string query = "SELECT * FROM Propietario WHERE IdPropietario = @IdPropietario";
        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@IdPropietario", id);
                try
                {
                    conexion.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                IdPropietario = Convert.ToInt32(reader["IdPropietario"]),
                                Dni = reader["Dni"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener datos: " + ex.Message);
                }
            }
        }
        return propietario;
    }
    public void actualizar(Propietario propietario)
    {
        string query = @"UPDATE Propietario SET Dni = @Dni, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Telefono = @Telefono WHERE IdPropietario = @IdPropietario";
        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@IdPropietario", propietario.IdPropietario);
                comando.Parameters.AddWithValue("@Dni", propietario.Dni);
                comando.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                comando.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                comando.Parameters.AddWithValue("@Email", propietario.Email);
                comando.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    if(filasAfectadas>0) {
                        Console.WriteLine("Propietario actualizado con éxito");
                    } else {
                        Console.WriteLine("No se encontró el propietario a actualizar");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar: " + ex.Message);
                }
            }
        }
    }
    public void eliminar(int id)
    {
        string query = "DELETE FROM Propietario WHERE IdPropietario = @IdPropietario";
        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@IdPropietario", id);
                try
                {
                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    if(filasAfectadas>0) {
                        Console.WriteLine("Propietario eliminado con éxito");
                    } else {
                        Console.WriteLine("No se encontró el propietario a eliminar");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar: " + ex.Message);
                }
            }
        }
    }
    public Propietario obtenerPorDni(string dni)
    {
        Propietario propietario = null;
        string query = "SELECT * FROM Propietario WHERE Dni = @Dni";
        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Dni", dni);
                try
                {
                    conexion.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                IdPropietario = Convert.ToInt32(reader["IdPropietario"]),
                                Dni = reader["Dni"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener por DNI: " + ex.Message);
                }
            }
        }
        return propietario;
    }
}

