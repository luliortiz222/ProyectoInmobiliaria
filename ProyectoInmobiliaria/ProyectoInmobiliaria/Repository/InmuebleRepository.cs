using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;

namespace ProyectoInmobiliaria.Repository   
{
    public class InmuebleRepository
    {
        private string _cadenaDeConexion;
        private PropietarioRepository _propietarioRepository;
        public InmuebleRepository(string cadenaDeConexion)
        {
            _cadenaDeConexion = cadenaDeConexion;
        }

        public void Guardar(Inmueble inmueble)
        {
            string query = @"INSERT INTO Inmueble (Direccion, Cupo, Coordenadas, PrecioPorDia, ImagenPortada,Estado, IdPropietario, IdTipoInmueble) 
                            VALUES (@Direccion, @Cupo, @Coordenadas, @PrecioPorDia, @ImagenPortada, @Estado, @IdPropietario, @IdTipoInmueble)";
            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    comando.Parameters.AddWithValue("@Cupo", inmueble.Cupo);
                    comando.Parameters.AddWithValue("@Coordenadas", inmueble.Coordenadas);
                    comando.Parameters.AddWithValue("@PrecioPorDia", inmueble.PrecioPorDia);
                    comando.Parameters.AddWithValue("@ImagenPortada", inmueble.ImagenPortada);
                    comando.Parameters.AddWithValue("@Estado", inmueble.Estado);
                    comando.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
                    comando.Parameters.AddWithValue("@IdTipoInmueble", inmueble.IdTipoInmueble);
                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                        Console.WriteLine("Inmueble guardado con éxito");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al guardar inmueble: " + ex.Message);
                    }
                }
            }
        }

        public void Eliminar(int idInmueble)
        {
            string query = "DELETE FROM Inmueble WHERE IdInmueble = @IdInmueble";
            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    try
                    {
                        conexion.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Inmueble eliminado con éxito");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el inmueble con el Id proporcionado");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al eliminar inmueble: " + ex.Message);
                    }
                }
            }
        }

        public List<Inmueble> ObtenerTodos()
        {
            List<Inmueble> inmuebles = new List<Inmueble>();
            string query = "SELECT * FROM Inmueble";
            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
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
                                Inmueble inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    Direccion = reader.GetString("Direccion"),
                                    Cupo = reader.GetInt32("Cupo"),
                                    Coordenadas = reader.GetString("Coordenadas"),
                                    PrecioPorDia = reader.GetDecimal("PrecioPorDia"),
                                    ImagenPortada = reader.GetString("ImagenPortada"),
                                    Estado = reader.GetBoolean("Estado"),
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                    //Dueño = _propietarioRepository.ObtenerPorId(reader.GetInt32("IdPropietario")),
                                    //Tipo = _tipoInmuebleRepository.ObtenerPorId(reader.GetInt32("IdTipoInmueble"))
                                    Dueño = new Propietario
                                    {
                                        Nombre = reader.GetString("Nombre"),
                                        Apellido = reader.GetString("Apellido")
                                    },
                                    Tipo = new TipoInmueble
                                    {
                                        Nombre = reader.GetString("Nombre")
                                    }
                                };
                                inmuebles.Add(inmueble);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener inmuebles: " + ex.Message);
                    }
                }
            }
            return inmuebles;
        }

        public void Actualizar(Inmueble inmueble)
        {
            string query = @"UPDATE Inmueble 
                            SET Direccion = @Direccion, 
                                Cupo = @Cupo, 
                                Coordenadas = @Coordenadas, 
                                PrecioPorDia = @PrecioPorDia, 
                                ImagenPortada = @ImagenPortada, 
                                Estado = @Estado, 
                                IdPropietario = @IdPropietario, 
                                IdTipoInmueble = @IdTipoInmueble 
                            WHERE IdInmueble = @IdInmueble";
            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    comando.Parameters.AddWithValue("@Cupo", inmueble.Cupo);
                    comando.Parameters.AddWithValue("@Coordenadas", inmueble.Coordenadas);
                    comando.Parameters.AddWithValue("@PrecioPorDia", inmueble.PrecioPorDia);
                    comando.Parameters.AddWithValue("@ImagenPortada", inmueble.ImagenPortada);
                    comando.Parameters.AddWithValue("@Estado", inmueble.Estado);
                    comando.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
                    comando.Parameters.AddWithValue("@IdTipoInmueble", inmueble.IdTipoInmueble);
                    comando.Parameters.AddWithValue("@IdInmueble", inmueble.IdInmueble);
                    try
                    {
                        conexion.Open();
                        int filasAfectadas = comando.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Inmueble actualizado con éxito");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el inmueble con el Id proporcionado");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al actualizar inmueble: " + ex.Message);
                    }
                }
            }
        }

        public Inmueble ObtenerPorId(int idInmueble)
        {
            Inmueble inmueble = null;
            string query = "SELECT * FROM Inmueble WHERE IdInmueble = @Id";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", idInmueble);
                    try
                    {
                        conexion.Open();
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32("IdInmueble"),
                                    Direccion = reader.GetString("Direccion"),
                                    Cupo = reader.GetInt32("Cupo"),
                                    Coordenadas = reader.GetString("Coordenadas"),
                                    PrecioPorDia = reader.GetDecimal("PrecioPorDia"),
                                    ImagenPortada = reader.IsDBNull(reader.GetOrdinal("ImagenPortada")) ? "" : reader.GetString("ImagenPortada"),
                                    Estado = reader.GetBoolean("Estado"),
                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble")
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener inmueble por ID: " + ex.Message);
                    }
                }
            }
            return inmueble;
        }
    }
}
