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

            string query = @"
        SELECT 
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,

            p.IdPropietario,
            p.Nombre AS NombrePropietario,
            p.Apellido AS ApellidoPropietario,

            t.IdTipoInmueble,
            t.Nombre AS NombreTipo

        FROM Inmueble i
        INNER JOIN Propietario p 
            ON i.IdPropietario = p.IdPropietario
        INNER JOIN TipoInmueble t 
            ON i.IdTipoInmueble = t.IdTipoInmueble";

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

                                    ImagenPortada = reader.IsDBNull(
                                        reader.GetOrdinal("ImagenPortada"))
                                        ? ""
                                        : reader.GetString("ImagenPortada"),

                                    Estado = reader.GetBoolean("Estado"),

                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                    Dueño = new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    },

                                    Tipo = new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("NombreTipo")
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

        public List<Inmueble> ObtenerPorEstado(bool estado)
        {
            List<Inmueble> inmuebles = new List<Inmueble>();

            string query = @"
        SELECT 
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,

            p.IdPropietario,
            p.Nombre AS NombrePropietario,
            p.Apellido AS ApellidoPropietario,

            t.IdTipoInmueble,
            t.Nombre AS NombreTipo

        FROM Inmueble i
        INNER JOIN Propietario p 
            ON i.IdPropietario = p.IdPropietario
        INNER JOIN TipoInmueble t 
            ON i.IdTipoInmueble = t.IdTipoInmueble

        WHERE i.Estado = @Estado";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Estado", estado);

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

                                    ImagenPortada = reader.IsDBNull(
                                        reader.GetOrdinal("ImagenPortada"))
                                        ? ""
                                        : reader.GetString("ImagenPortada"),

                                    Estado = reader.GetBoolean("Estado"),

                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                    Dueño = new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    },

                                    Tipo = new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("NombreTipo")
                                    }
                                };

                                inmuebles.Add(inmueble);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al filtrar inmuebles por estado: " + ex.Message);
                    }
                }
            }

            return inmuebles;
        }

        public List<Inmueble> ObtenerPorPropietario(int idPropietario)
        {
            List<Inmueble> inmuebles = new List<Inmueble>();

            string query = @"
        SELECT 
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,

            p.Nombre AS NombrePropietario,
            p.Apellido AS ApellidoPropietario,

            t.Nombre AS NombreTipo

        FROM Inmueble i

        INNER JOIN Propietario p 
            ON i.IdPropietario = p.IdPropietario

        INNER JOIN TipoInmueble t 
            ON i.IdTipoInmueble = t.IdTipoInmueble

        WHERE i.IdPropietario = @IdPropietario";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdPropietario", idPropietario);

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

                                    ImagenPortada = reader.IsDBNull(
                                        reader.GetOrdinal("ImagenPortada"))
                                        ? ""
                                        : reader.GetString("ImagenPortada"),

                                    Estado = reader.GetBoolean("Estado"),

                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                    Dueño = new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    },

                                    Tipo = new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("NombreTipo")
                                    }
                                };

                                inmuebles.Add(inmueble);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener inmuebles por propietario: " + ex.Message);
                    }
                }
            }

            return inmuebles;
        }

        public List<Inmueble> ObtenerMasReservados()
        {
            List<Inmueble> inmuebles = new List<Inmueble>();

            DateTime fechaLimite = DateTime.Today.AddDays(-365);

            string query = @"
        SELECT 
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,

            p.Nombre AS NombrePropietario,
            p.Apellido AS ApellidoPropietario,

            t.Nombre AS NombreTipo,

            COUNT(r.IdReserva) AS CantidadReservas

        FROM Inmueble i

        INNER JOIN Propietario p
            ON i.IdPropietario = p.IdPropietario

        INNER JOIN TipoInmueble t
            ON i.IdTipoInmueble = t.IdTipoInmueble

        LEFT JOIN Reserva r
            ON i.IdInmueble = r.IdInmueble
            AND r.FechaDesde >= @FechaLimite

        GROUP BY
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,
            p.Nombre,
            p.Apellido,
            t.Nombre

        ORDER BY CantidadReservas DESC";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@FechaLimite", fechaLimite);

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

                                    ImagenPortada = reader.IsDBNull(
                                        reader.GetOrdinal("ImagenPortada"))
                                        ? ""
                                        : reader.GetString("ImagenPortada"),

                                    Estado = reader.GetBoolean("Estado"),

                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                    CantidadReservas = reader.GetInt32("CantidadReservas"),

                                    Dueño = new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    },

                                    Tipo = new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("NombreTipo")
                                    }
                                };

                                inmuebles.Add(inmueble);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener inmuebles más reservados: " + ex.Message);
                    }
                }
            }

            return inmuebles;
        }

        public List<Inmueble> ObtenerSinReservas(int dias)
        {
            List<Inmueble> inmuebles = new List<Inmueble>();

            DateTime fechaLimite = DateTime.Today.AddDays(-dias);

            string query = @"
        SELECT 
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,

            p.Nombre AS NombrePropietario,
            p.Apellido AS ApellidoPropietario,

            t.Nombre AS NombreTipo

        FROM Inmueble i

        INNER JOIN Propietario p
            ON i.IdPropietario = p.IdPropietario

        INNER JOIN TipoInmueble t
            ON i.IdTipoInmueble = t.IdTipoInmueble

        WHERE NOT EXISTS
        (
            SELECT 1
            FROM Reserva r
            WHERE r.IdInmueble = i.IdInmueble
            AND r.FechaDesde >= @FechaLimite
        )";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@FechaLimite", fechaLimite);

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

                                    ImagenPortada = reader.IsDBNull(
                                        reader.GetOrdinal("ImagenPortada"))
                                        ? ""
                                        : reader.GetString("ImagenPortada"),

                                    Estado = reader.GetBoolean("Estado"),

                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                    Dueño = new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    },

                                    Tipo = new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("NombreTipo")
                                    }
                                };

                                inmuebles.Add(inmueble);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener inmuebles sin reservas: " + ex.Message);
                    }
                }
            }

            return inmuebles;
        }

        public List<Inmueble> ObtenerDisponiblesEntreFechas(
    DateTime fechaDesde,
    DateTime fechaHasta)
        {
            List<Inmueble> inmuebles = new List<Inmueble>();

            string query = @"
        SELECT 
            i.IdInmueble,
            i.Direccion,
            i.Cupo,
            i.Coordenadas,
            i.PrecioPorDia,
            i.ImagenPortada,
            i.Estado,
            i.IdPropietario,
            i.IdTipoInmueble,

            p.Nombre AS NombrePropietario,
            p.Apellido AS ApellidoPropietario,

            t.Nombre AS NombreTipo

        FROM Inmueble i

        INNER JOIN Propietario p
            ON i.IdPropietario = p.IdPropietario

        INNER JOIN TipoInmueble t
            ON i.IdTipoInmueble = t.IdTipoInmueble

        WHERE i.Estado = 1

        AND NOT EXISTS
        (
            SELECT 1
            FROM Reserva r
            WHERE r.IdInmueble = i.IdInmueble
              AND r.FechaDesde < @FechaHasta
              AND r.FechaHasta > @FechaDesde
        )";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@FechaDesde", fechaDesde);
                    comando.Parameters.AddWithValue("@FechaHasta", fechaHasta);

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

                                    ImagenPortada = reader.IsDBNull(
                                        reader.GetOrdinal("ImagenPortada"))
                                        ? ""
                                        : reader.GetString("ImagenPortada"),

                                    Estado = reader.GetBoolean("Estado"),

                                    IdPropietario = reader.GetInt32("IdPropietario"),
                                    IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                    Dueño = new Propietario
                                    {
                                        IdPropietario = reader.GetInt32("IdPropietario"),
                                        Nombre = reader.GetString("NombrePropietario"),
                                        Apellido = reader.GetString("ApellidoPropietario")
                                    },

                                    Tipo = new TipoInmueble
                                    {
                                        IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                        Nombre = reader.GetString("NombreTipo")
                                    }
                                };

                                inmuebles.Add(inmueble);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "Error al obtener inmuebles disponibles entre fechas: "
                            + ex.Message);
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
            string query = @"
        SELECT i.IdInmueble, i.Direccion, i.Cupo, i.Coordenadas, i.PrecioPorDia, 
               i.ImagenPortada, i.Estado, i.IdPropietario, i.IdTipoInmueble,
               p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario, p.Dni,
               t.Nombre AS NombreTipo
        FROM Inmueble i
        INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario
        INNER JOIN TipoInmueble t ON i.IdTipoInmueble = t.IdTipoInmueble
        WHERE i.IdInmueble = @Id";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Id", idInmueble);
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
                                Coordenadas = reader.IsDBNull(reader.GetOrdinal("Coordenadas")) ? "" : reader.GetString("Coordenadas"),
                                PrecioPorDia = reader.GetDecimal("PrecioPorDia"),
                                ImagenPortada = reader.IsDBNull(reader.GetOrdinal("ImagenPortada")) ? "" : reader.GetString("ImagenPortada"),
                                Estado = reader.GetBoolean("Estado"),
                                IdPropietario = reader.GetInt32("IdPropietario"),
                                IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),

                                Dueño = new Propietario
                                {
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario"),
                                    Dni = reader.GetString("Dni"),
                                    
                                },
                                Tipo = new TipoInmueble
                                {
                                    Nombre = reader.GetString("NombreTipo")
                                }
                            };
                        }
                    }
                }
            }
            return inmueble;
        }
    }
}
