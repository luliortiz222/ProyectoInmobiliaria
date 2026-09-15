using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;

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

    public List<Inquilino> ObtenerPaginados(string busqueda, int pagina, int cantidadPorPagina)
    {
        List<Inquilino> lista = new List<Inquilino>();

        int desplazamiento = (pagina - 1) * cantidadPorPagina;

        string query = @"
        SELECT *
        FROM Inquilino
        WHERE Nombre LIKE @Busqueda
           OR Apellido LIKE @Busqueda
           OR Dni LIKE @Busqueda
        ORDER BY IdInquilino
        LIMIT @CantidadPorPagina OFFSET @Desplazamiento";

        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Busqueda", "%" + (busqueda ?? "") + "%");
                comando.Parameters.AddWithValue("@CantidadPorPagina", cantidadPorPagina);
                comando.Parameters.AddWithValue("@Desplazamiento", desplazamiento);

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
                    Console.WriteLine("Error al obtener inquilinos paginados: " + ex.Message);
                }
            }
        }

        return lista;
    }

    public int ContarInquilinos(string busqueda)
    {
        int cantidad = 0;

        string query = @"
        SELECT COUNT(*)
        FROM Inquilino
        WHERE Nombre LIKE @Busqueda
           OR Apellido LIKE @Busqueda
           OR Dni LIKE @Busqueda";

        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Busqueda", "%" + (busqueda ?? "") + "%");

                try
                {
                    conexion.Open();
                    cantidad = Convert.ToInt32(comando.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al contar inquilinos: " + ex.Message);
                }
            }
        }

        return cantidad;
    }

    public Inquilino ObtenerPorId(int id)
    {
        Inquilino i = null;
        string query = "SELECT * FROM Inquilino WHERE IdInquilino = @Id";

        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Id", id);

                try
                {
                    conexion.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            i = new Inquilino
                            {
                                IdInquilino = Convert.ToInt32(reader["IdInquilino"]),
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
                    Console.WriteLine("Error al obtener por ID: " + ex.Message);
                }
            }
        }
        return i;
    }

    public void Modificar(Inquilino inquilino)
    {
        string query = @"UPDATE Inquilino 
                        SET Dni = @Dni, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Telefono = @Telefono 
                        WHERE IdInquilino = @Id";

        using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
        {
            using (MySqlCommand comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@Id", inquilino.IdInquilino);
                comando.Parameters.AddWithValue("@Dni", inquilino.Dni);
                comando.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                comando.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                comando.Parameters.AddWithValue("@Email", inquilino.Email);
                comando.Parameters.AddWithValue("@Telefono", inquilino.Telefono);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al modificar inquilino: " + ex.Message);
                }
            }
        }
    }

    public bool Eliminar(int id)
{
    string query = "DELETE FROM Inquilino WHERE IdInquilino = @Id";

    using (MySqlConnection conexion = new MySqlConnection(_cadenaConexion))
    {
        using (MySqlCommand comando = new MySqlCommand(query, conexion))
        {
            comando.Parameters.AddWithValue("@Id", id);

            try
            {
                conexion.Open();

                int filasAfectadas = comando.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar inquilino: " + ex.Message);
                return false;
            }
        }
    }
}
}