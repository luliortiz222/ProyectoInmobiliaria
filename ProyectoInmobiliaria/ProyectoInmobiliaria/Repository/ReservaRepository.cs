using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;

namespace ProyectoInmobiliaria.Repository
{
    public class ReservaRepository
    {
        private readonly string connectionString;

        public ReservaRepository(string cadenaConexion)
        {
            connectionString = cadenaConexion;
        }

        // Obtener todas las reservas
        public List<Reserva> ObtenerTodos()
        {
            List<Reserva> lista = new List<Reserva>();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"SELECT IdReserva, IdInquilino, IdInmueble,
                                      MontoPorDia, FechaDesde, FechaHasta
                               FROM Reserva";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reserva reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32("IdReserva"),
                                IdInquilino = reader.GetInt32("IdInquilino"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                MontoPorDia = reader.GetDecimal("MontoPorDia"),
                                FechaDesde = reader.GetDateTime("FechaDesde"),
                                FechaHasta = reader.GetDateTime("FechaHasta")
                            };

                            lista.Add(reserva);
                        }
                    }
                }
            }

            return lista;
        }

        public Reserva ObtenerPorId(int id)
        {
            Reserva reserva = null;

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"SELECT IdReserva, IdInquilino, IdInmueble,
                                      MontoPorDia, FechaDesde, FechaHasta
                               FROM Reserva
                               WHERE IdReserva = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32("IdReserva"),
                                IdInquilino = reader.GetInt32("IdInquilino"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                MontoPorDia = reader.GetDecimal("MontoPorDia"),
                                FechaDesde = reader.GetDateTime("FechaDesde"),
                                FechaHasta = reader.GetDateTime("FechaHasta")
                            };
                        }
                    }
                }
            }

            return reserva;
        }

        public List<Reserva> ObtenerPaginados(
    string busqueda,
    int? idInquilino,
    int? idInmueble,
    int pagina,
    int cantidadPorPagina)
        {
            List<Reserva> lista = new List<Reserva>();

            int desplazamiento = (pagina - 1) * cantidadPorPagina;

            string sql = @"
        SELECT 
            r.IdReserva,
            r.IdInquilino,
            r.IdInmueble,
            r.MontoPorDia,
            r.FechaDesde,
            r.FechaHasta,
            r.IdUsuarioCreador,
            r.IdUsuarioFinalizador,
            r.FechaFinalizacion,

            CONCAT(i.Nombre, ' ', i.Apellido) AS NombreInquilino,
            inm.Direccion AS DireccionInmueble

        FROM Reserva r

        INNER JOIN Inquilino i
            ON r.IdInquilino = i.IdInquilino

        INNER JOIN Inmueble inm
            ON r.IdInmueble = inm.IdInmueble

        WHERE (
            CAST(r.IdReserva AS CHAR) LIKE @Busqueda
            OR CAST(r.IdInquilino AS CHAR) LIKE @Busqueda
            OR CAST(r.IdInmueble AS CHAR) LIKE @Busqueda
            OR CAST(r.MontoPorDia AS CHAR) LIKE @Busqueda
            OR CONCAT(i.Nombre, ' ', i.Apellido) LIKE @Busqueda
            OR inm.Direccion LIKE @Busqueda
        )

        AND (@IdInquilino IS NULL OR r.IdInquilino = @IdInquilino)

        AND (@IdInmueble IS NULL OR r.IdInmueble = @IdInmueble)

        ORDER BY r.FechaDesde DESC

        LIMIT @CantidadPorPagina
        OFFSET @Desplazamiento";

            using (MySqlConnection conexion =
                   new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlCommand comando =
                       new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@Busqueda",
                        "%" + (busqueda ?? "") + "%");

                    comando.Parameters.AddWithValue(
                        "@IdInquilino",
                        idInquilino.HasValue
                            ? idInquilino.Value
                            : DBNull.Value);

                    comando.Parameters.AddWithValue(
                        "@IdInmueble",
                        idInmueble.HasValue
                            ? idInmueble.Value
                            : DBNull.Value);

                    comando.Parameters.AddWithValue(
                        "@CantidadPorPagina",
                        cantidadPorPagina);

                    comando.Parameters.AddWithValue(
                        "@Desplazamiento",
                        desplazamiento);

                    using (MySqlDataReader reader =
                           comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reserva reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32("IdReserva"),

                                IdInquilino = reader.GetInt32("IdInquilino"),

                                IdInmueble = reader.GetInt32("IdInmueble"),

                                MontoPorDia = reader.GetDecimal("MontoPorDia"),

                                FechaDesde = reader.GetDateTime("FechaDesde"),

                                FechaHasta = reader.GetDateTime("FechaHasta"),

                                IdUsuarioCreador = reader.IsDBNull(
                                    reader.GetOrdinal("IdUsuarioCreador"))
                                    ? 0
                                    : reader.GetInt32("IdUsuarioCreador"),

                                IdUsuarioFinalizador = reader.IsDBNull(
                                    reader.GetOrdinal("IdUsuarioFinalizador"))
                                    ? (int?)null
                                    : reader.GetInt32("IdUsuarioFinalizador"),

                                FechaFinalizacion = reader.IsDBNull(
                                    reader.GetOrdinal("FechaFinalizacion"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime("FechaFinalizacion"),

                                NombreInquilino =
                                    reader.GetString("NombreInquilino"),

                                DireccionInmueble =
                                    reader.GetString("DireccionInmueble")
                            };

                            lista.Add(reserva);
                        }
                    }
                }
            }

            return lista;
        }

        public int ContarReservas(
    string busqueda,
    int? idInquilino,
    int? idInmueble)
        {
            int cantidad = 0;

            string sql = @"
        SELECT COUNT(*)

        FROM Reserva r

        INNER JOIN Inquilino i
            ON r.IdInquilino = i.IdInquilino

        INNER JOIN Inmueble inm
            ON r.IdInmueble = inm.IdInmueble

        WHERE (
            CAST(r.IdReserva AS CHAR) LIKE @Busqueda
            OR CAST(r.IdInquilino AS CHAR) LIKE @Busqueda
            OR CAST(r.IdInmueble AS CHAR) LIKE @Busqueda
            OR CAST(r.MontoPorDia AS CHAR) LIKE @Busqueda
            OR CONCAT(i.Nombre, ' ', i.Apellido) LIKE @Busqueda
            OR inm.Direccion LIKE @Busqueda
        )

        AND (@IdInquilino IS NULL OR r.IdInquilino = @IdInquilino)

        AND (@IdInmueble IS NULL OR r.IdInmueble = @IdInmueble)";

            using (MySqlConnection conexion =
                   new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlCommand comando =
                       new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@Busqueda",
                        "%" + (busqueda ?? "") + "%");

                    comando.Parameters.AddWithValue(
                        "@IdInquilino",
                        idInquilino.HasValue
                            ? idInquilino.Value
                            : DBNull.Value);

                    comando.Parameters.AddWithValue(
                        "@IdInmueble",
                        idInmueble.HasValue
                            ? idInmueble.Value
                            : DBNull.Value);

                    cantidad = Convert.ToInt32(
                        comando.ExecuteScalar());
                }
            }

            return cantidad;
        }
        // Verificar si un inmueble está disponible para ciertas fechas
        public bool EstaDisponible(int idInmueble, DateTime fechaDesde, DateTime fechaHasta)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"SELECT COUNT(*)
                               FROM Reserva
                               WHERE IdInmueble = @idInmueble
                               AND FechaDesde < @fechaHasta
                               AND FechaHasta > @fechaDesde";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idInmueble", idInmueble);
                    comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);

                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());

                    return cantidad == 0;
                }
            }
        }

        public List<Reserva> ObtenerVigentes()
        {
            List<Reserva> reservas = new List<Reserva>();

            string query = @"
        SELECT
            IdReserva,
            IdInquilino,
            IdInmueble,
            MontoPorDia,
            FechaDesde,
            FechaHasta,
            IdUsuarioCreador
        FROM Reserva
        WHERE FechaDesde <= CURDATE()
          AND FechaHasta >= CURDATE()
        ORDER BY FechaDesde ASC";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reserva reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32("IdReserva"),
                                IdInquilino = reader.GetInt32("IdInquilino"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                MontoPorDia = reader.GetDecimal("MontoPorDia"),
                                FechaDesde = reader.GetDateTime("FechaDesde"),
                                FechaHasta = reader.GetDateTime("FechaHasta"),
                                IdUsuarioCreador = reader.IsDBNull(reader.GetOrdinal("IdUsuarioCreador"))
                                ? 0
                                : reader.GetInt32("IdUsuarioCreador")
                            };

                            reservas.Add(reserva);
                        }
                    }
                }
            }

            return reservas;
        }

        public List<Reserva> ObtenerQueTerminanEnDias(int dias)
        {
            List<Reserva> reservas = new List<Reserva>();

            DateTime fechaObjetivo = DateTime.Today.AddDays(dias);

            string query = @"
        SELECT
            IdReserva,
            IdInquilino,
            IdInmueble,
            MontoPorDia,
            FechaDesde,
            FechaHasta,
            IdUsuarioCreador
        FROM Reserva
        WHERE FechaHasta = @FechaObjetivo
        ORDER BY FechaHasta ASC";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@FechaObjetivo", fechaObjetivo);

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reserva reserva = new Reserva
                            {
                                IdReserva = reader.GetInt32("IdReserva"),
                                IdInquilino = reader.GetInt32("IdInquilino"),
                                IdInmueble = reader.GetInt32("IdInmueble"),
                                MontoPorDia = reader.GetDecimal("MontoPorDia"),
                                FechaDesde = reader.GetDateTime("FechaDesde"),
                                FechaHasta = reader.GetDateTime("FechaHasta"),
                                IdUsuarioCreador = reader.GetInt32("IdUsuarioCreador")
                            };

                            reservas.Add(reserva);
                        }
                    }
                }
            }

            return reservas;
        }

        // Guardar una nueva reserva
        public bool Guardar(Reserva reserva)
        {
            // Primero verificar que las fechas sean válidas
            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                return false;
            }

            // Verificar que el inmueble esté disponible
            if (!EstaDisponible(
                reserva.IdInmueble,
                reserva.FechaDesde,
                reserva.FechaHasta))
            {
                return false;
            }

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"INSERT INTO Reserva
                               (IdInquilino, IdInmueble, MontoPorDia, FechaDesde, FechaHasta, IdUsuarioCreador)
                               VALUES
                               (@idInquilino, @idInmueble, @montoPorDia, @fechaDesde, @fechaHasta, @idUsuarioCreador)";

                try
                {
                    using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@idInquilino", reserva.IdInquilino);
                        comando.Parameters.AddWithValue("@idInmueble", reserva.IdInmueble);
                        comando.Parameters.AddWithValue("@montoPorDia", reserva.MontoPorDia);
                        comando.Parameters.AddWithValue("@fechaDesde", reserva.FechaDesde);
                        comando.Parameters.AddWithValue("@fechaHasta", reserva.FechaHasta);
                        comando.Parameters.AddWithValue("@idUsuarioCreador", reserva.IdUsuarioCreador);
                        
                        comando.ExecuteNonQuery();
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return true;
        }

        // Editar una reserva
        public bool Editar(Reserva reserva)
        {
            // Verificar que las fechas sean válidas
            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                return false;
            }

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                // Verificar si existe otra reserva que se superponga
                string sqlVerificar = @"SELECT COUNT(*)
                                        FROM Reserva
                                        WHERE IdInmueble = @idInmueble
                                        AND IdReserva <> @idReserva
                                        AND FechaDesde < @fechaHasta
                                        AND FechaHasta > @fechaDesde";

                using (MySqlCommand comandoVerificar = new MySqlCommand(sqlVerificar, conexion))
                {
                    comandoVerificar.Parameters.AddWithValue("@idInmueble", reserva.IdInmueble);
                    comandoVerificar.Parameters.AddWithValue("@idReserva", reserva.IdReserva);
                    comandoVerificar.Parameters.AddWithValue("@fechaDesde", reserva.FechaDesde);
                    comandoVerificar.Parameters.AddWithValue("@fechaHasta", reserva.FechaHasta);

                    int cantidad = Convert.ToInt32(comandoVerificar.ExecuteScalar());

                    if (cantidad > 0)
                    {
                        return false;
                    }
                }

                string sql = @"UPDATE Reserva
                               SET IdInquilino = @idInquilino,
                                   IdInmueble = @idInmueble,
                                   MontoPorDia = @montoPorDia,
                                   FechaDesde = @fechaDesde,
                                   FechaHasta = @fechaHasta
                               WHERE IdReserva = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idInquilino", reserva.IdInquilino);
                    comando.Parameters.AddWithValue("@idInmueble", reserva.IdInmueble);
                    comando.Parameters.AddWithValue("@montoPorDia", reserva.MontoPorDia);
                    comando.Parameters.AddWithValue("@fechaDesde", reserva.FechaDesde);
                    comando.Parameters.AddWithValue("@fechaHasta", reserva.FechaHasta);
                    comando.Parameters.AddWithValue("@id", reserva.IdReserva);

                    comando.ExecuteNonQuery();
                }
            }

            return true;
        }

        public bool Finalizar(int idReserva, int idUsuarioFinalizador)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"
            UPDATE Reserva
            SET IdUsuarioFinalizador = @IdUsuarioFinalizador,
                FechaFinalizacion = CURDATE()
            WHERE IdReserva = @IdReserva
              AND IdUsuarioFinalizador IS NULL";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuarioFinalizador", idUsuarioFinalizador);
                    comando.Parameters.AddWithValue("@IdReserva", idReserva);

                    int filasAfectadas = comando.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
        }

        // Eliminar una reserva
        public bool Eliminar(int id)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sqlVerificar = @"
            SELECT COUNT(*)
            FROM Pago
            WHERE IdReserva = @IdReserva";

                using (MySqlCommand comandoVerificar = new MySqlCommand(sqlVerificar, conexion))
                {
                    comandoVerificar.Parameters.AddWithValue("@IdReserva", id);

                    int cantidadPagos = Convert.ToInt32(comandoVerificar.ExecuteScalar());

                    if (cantidadPagos > 0)
                    {
                        return false;
                    }
                }

                string sql = "DELETE FROM Reserva WHERE IdReserva = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    int filasAfectadas = comando.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
        }
    }
}