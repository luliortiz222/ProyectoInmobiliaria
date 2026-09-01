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

        // Buscar una reserva por ID
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
                               (IdInquilino, IdInmueble, MontoPorDia, FechaDesde, FechaHasta)
                               VALUES
                               (@idInquilino, @idInmueble, @montoPorDia, @fechaDesde, @fechaHasta)";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@idInquilino", reserva.IdInquilino);
                    comando.Parameters.AddWithValue("@idInmueble", reserva.IdInmueble);
                    comando.Parameters.AddWithValue("@montoPorDia", reserva.MontoPorDia);
                    comando.Parameters.AddWithValue("@fechaDesde", reserva.FechaDesde);
                    comando.Parameters.AddWithValue("@fechaHasta", reserva.FechaHasta);

                    comando.ExecuteNonQuery();
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

        // Eliminar una reserva
        public void Eliminar(int id)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = "DELETE FROM Reserva WHERE IdReserva = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}