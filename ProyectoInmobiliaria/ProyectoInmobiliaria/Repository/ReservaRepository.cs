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

                string sql = @"
            SELECT
                IdReserva,
                IdInquilino,
                IdInmueble,
                MontoPorDia,
                FechaDesde,
                FechaHasta,
                IdUsuarioCreador,
                IdUsuarioFinalizador,
                FechaFinalizacion
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
                                    : reader.GetDateTime("FechaFinalizacion")
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
        public int Guardar(
    Reserva reserva,
    decimal porcentajeReserva)
        {
            // Verificar que las fechas sean válidas
            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                return 0;
            }

            // Verificar que el inmueble esté disponible
            if (!EstaDisponible(
                reserva.IdInmueble,
                reserva.FechaDesde,
                reserva.FechaHasta))
            {
                return 0;
            }

            // Verificar que el porcentaje sea válido
            if (porcentajeReserva < 0 || porcentajeReserva > 100)
            {
                return 0;
            }

            using (MySqlConnection conexion =
                   new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlTransaction transaccion =
                       conexion.BeginTransaction())
                {
                    try
                    {
                        // se crea la reserva

                        string sqlReserva = @"
                    INSERT INTO Reserva
                    (
                        IdInquilino,
                        IdInmueble,
                        MontoPorDia,
                        FechaDesde,
                        FechaHasta,
                        IdUsuarioCreador
                    )
                    VALUES
                    (
                        @IdInquilino,
                        @IdInmueble,
                        @MontoPorDia,
                        @FechaDesde,
                        @FechaHasta,
                        @IdUsuarioCreador
                    )";

                        int idReserva;

                        using (MySqlCommand comandoReserva =
                               new MySqlCommand(
                                   sqlReserva,
                                   conexion,
                                   transaccion))
                        {
                            comandoReserva.Parameters.AddWithValue(
                                "@IdInquilino",
                                reserva.IdInquilino);

                            comandoReserva.Parameters.AddWithValue(
                                "@IdInmueble",
                                reserva.IdInmueble);

                            comandoReserva.Parameters.AddWithValue(
                                "@MontoPorDia",
                                reserva.MontoPorDia);

                            comandoReserva.Parameters.AddWithValue(
                                "@FechaDesde",
                                reserva.FechaDesde.Date);

                            comandoReserva.Parameters.AddWithValue(
                                "@FechaHasta",
                                reserva.FechaHasta.Date);

                            comandoReserva.Parameters.AddWithValue(
                                "@IdUsuarioCreador",
                                reserva.IdUsuarioCreador);

                            comandoReserva.ExecuteNonQuery();

                            idReserva =
                                Convert.ToInt32(
                                    comandoReserva.LastInsertedId);
                        }

                        // se calcula el pago inicial 

                        int dias =
                            (reserva.FechaHasta.Date -
                             reserva.FechaDesde.Date).Days;

                        decimal totalAlquiler =
                            dias * reserva.MontoPorDia;

                        decimal importeInicial =
                            Math.Round(
                                totalAlquiler *
                                porcentajeReserva / 100m,
                                2);

                        // se crea el pago inicial

                        if (importeInicial > 0)
                        {
                            string sqlPago = @"
                        INSERT INTO Pago
                        (
                            IdReserva,
                            Concepto,
                            FechaPago,
                            Importe,
                            Estado,
                            IdUsuarioCreador
                        )
                        VALUES
                        (
                            @IdReserva,
                            @Concepto,
                            @FechaPago,
                            @Importe,
                            @Estado,
                            @IdUsuarioCreador
                        )";

                            using (MySqlCommand comandoPago =
                                   new MySqlCommand(
                                       sqlPago,
                                       conexion,
                                       transaccion))
                            {
                                comandoPago.Parameters.AddWithValue(
                                    "@IdReserva",
                                    idReserva);

                                comandoPago.Parameters.AddWithValue(
                                    "@Concepto",
                                    "Pago inicial de reserva");

                                comandoPago.Parameters.AddWithValue(
                                    "@FechaPago",
                                    DateTime.Today);

                                comandoPago.Parameters.AddWithValue(
                                    "@Importe",
                                    importeInicial);

                                comandoPago.Parameters.AddWithValue(
                                    "@Estado",
                                    true);

                                comandoPago.Parameters.AddWithValue(
                                    "@IdUsuarioCreador",
                                    reserva.IdUsuarioCreador);

                                comandoPago.ExecuteNonQuery();
                            }
                        }

                        // Confirmar

                        transaccion.Commit();

                        return idReserva;
                    }
                    catch (Exception ex)
                    {
                        // Si falla la reserva o el pago, no queda guardada ninguna de las dos cosas.

                        transaccion.Rollback();

                        Console.WriteLine(
                            "Error al guardar reserva y pago inicial: "
                            + ex.Message);

                        return 0;
                    }
                }
            }
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

        public decimal CalcularMulta(Reserva reserva, DateTime fechaTerminacion)
        {
            // Si no termina antes de la fecha original, no hay multa
            if (fechaTerminacion.Date >= reserva.FechaHasta.Date)
            {
                return 0;
            }

            // La fecha de terminación no puede ser anterior al inicio
            if (fechaTerminacion.Date < reserva.FechaDesde.Date)
            {
                return 0;
            }

            int diasTotales = (reserva.FechaHasta.Date - reserva.FechaDesde.Date).Days;
            int diasCumplidos = (fechaTerminacion.Date - reserva.FechaDesde.Date).Days;
            int diasRestantes = (reserva.FechaHasta.Date - fechaTerminacion.Date).Days;

            if (diasTotales <= 0 || diasRestantes <= 0)
            {
                return 0;
            }

            decimal porcentajeMulta;

            if (diasCumplidos < diasTotales / 2.0)
            {
                porcentajeMulta = 0.50m;
            }
            else
            {
                porcentajeMulta = 0.25m;
            }

            decimal multa = diasRestantes
                            * reserva.MontoPorDia
                            * porcentajeMulta;

            return Math.Round(multa, 2);
        }

        public bool FinalizarConMulta(
         int idReserva,
         DateTime fechaTerminacion,
         int idUsuarioFinalizador)
        {
            Reserva reserva = ObtenerPorId(idReserva);

            if (reserva == null)
            {
                return false;
            }

            // No permitir finalizar una reserva ya finalizada
            if (reserva.IdUsuarioFinalizador != null)
            {
                return false;
            }

            // La fecha efectiva debe estar dentro del período original
            if (fechaTerminacion.Date < reserva.FechaDesde.Date ||
                fechaTerminacion.Date > reserva.FechaHasta.Date)
            {
                return false;
            }

            decimal multa = CalcularMulta(reserva, fechaTerminacion);

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // Si existe multa, se registra como pago
                        if (multa > 0)
                        {
                            string sqlPago = @"
                        INSERT INTO Pago
                        (
                            IdReserva,
                            Concepto,
                            FechaPago,
                            Importe,
                            Estado,
                            IdUsuarioCreador
                        )
                        VALUES
                        (
                            @IdReserva,
                            @Concepto,
                            @FechaPago,
                            @Importe,
                            @Estado,
                            @IdUsuarioCreador
                        )";

                            using (MySqlCommand comandoPago =
                                   new MySqlCommand(sqlPago, conexion, transaccion))
                            {
                                comandoPago.Parameters.AddWithValue(
                                    "@IdReserva", idReserva);

                                comandoPago.Parameters.AddWithValue(
                                    "@Concepto",
                                    "Multa por finalización anticipada");

                                comandoPago.Parameters.AddWithValue(
                                    "@FechaPago",
                                    fechaTerminacion.Date);

                                comandoPago.Parameters.AddWithValue(
                                    "@Importe",
                                    multa);

                                comandoPago.Parameters.AddWithValue(
                                    "@Estado", true);

                                comandoPago.Parameters.AddWithValue(
                                    "@IdUsuarioCreador",
                                    idUsuarioFinalizador);

                                comandoPago.ExecuteNonQuery();
                            }
                        }

                        // Finalizar la reserva sin modificar FechaHasta
                        string sqlReserva = @"
                    UPDATE Reserva
                    SET IdUsuarioFinalizador = @IdUsuarioFinalizador,
                        FechaFinalizacion = @FechaFinalizacion
                    WHERE IdReserva = @IdReserva
                      AND IdUsuarioFinalizador IS NULL";

                        using (MySqlCommand comandoReserva =
                               new MySqlCommand(sqlReserva, conexion, transaccion))
                        {
                            comandoReserva.Parameters.AddWithValue(
                                "@IdUsuarioFinalizador",
                                idUsuarioFinalizador);

                            comandoReserva.Parameters.AddWithValue(
                                "@FechaFinalizacion",
                                fechaTerminacion.Date);

                            comandoReserva.Parameters.AddWithValue(
                                "@IdReserva",
                                idReserva);

                            int filasAfectadas = comandoReserva.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                transaccion.Rollback();
                                return false;
                            }
                        }

                        transaccion.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();

                        Console.WriteLine(
                            "Error al finalizar reserva con multa: "
                            + ex.Message);

                        return false;
                    }
                }
            }
        }

        public bool Renovar(
    int idReservaOriginal,
    DateTime nuevaFechaDesde,
    DateTime nuevaFechaHasta,
    decimal nuevoMontoPorDia,
    int idUsuarioCreador)
        {
            Reserva reservaOriginal = ObtenerPorId(idReservaOriginal);

            if (reservaOriginal == null)
            {
                return false;
            }

            // No se puede renovar una reserva ya finalizada
            if (reservaOriginal.IdUsuarioFinalizador != null)
            {
                return false;
            }

            // Las nuevas fechas deben ser válidas
            if (nuevaFechaDesde >= nuevaFechaHasta)
            {
                return false;
            }

            // La renovación debe comenzar desde el final de la reserva original
            if (nuevaFechaDesde.Date < reservaOriginal.FechaHasta.Date)
            {
                return false;
            }

            // El inmueble debe estar disponible para las nuevas fechas
            if (!EstaDisponible(
                reservaOriginal.IdInmueble,
                nuevaFechaDesde,
                nuevaFechaHasta))
            {
                return false;
            }

            using (MySqlConnection conexion =
                   new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"
            INSERT INTO Reserva
            (
                IdInquilino,
                IdInmueble,
                MontoPorDia,
                FechaDesde,
                FechaHasta,
                IdUsuarioCreador
            )
            VALUES
            (
                @IdInquilino,
                @IdInmueble,
                @MontoPorDia,
                @FechaDesde,
                @FechaHasta,
                @IdUsuarioCreador
            )";

                using (MySqlCommand comando =
                       new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@IdInquilino",
                        reservaOriginal.IdInquilino);

                    comando.Parameters.AddWithValue(
                        "@IdInmueble",
                        reservaOriginal.IdInmueble);

                    comando.Parameters.AddWithValue(
                        "@MontoPorDia",
                        nuevoMontoPorDia);

                    comando.Parameters.AddWithValue(
                        "@FechaDesde",
                        nuevaFechaDesde.Date);

                    comando.Parameters.AddWithValue(
                        "@FechaHasta",
                        nuevaFechaHasta.Date);

                    comando.Parameters.AddWithValue(
                        "@IdUsuarioCreador",
                        idUsuarioCreador);

                    int filasAfectadas = comando.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
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