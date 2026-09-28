using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;
using System;
using System.Collections.Generic;
using ProyectoInmobiliaria.models;

namespace ProyectoInmobiliaria.Repository
{
    public class PagoRepository
    {
        private readonly string cadenaDeConexion;
        public PagoRepository(string cadenaDeConexion)
        {
            this.cadenaDeConexion = cadenaDeConexion;
        }
        

        public void GuardarPago(Pago pago)
        {
            string query = "INSERT INTO Pago (IdReserva, Concepto, FechaPago, Importe, Estado, IdUsuarioCreador) VALUES (@idReserva, @concepto, @fechaPago, @importe, 1, @idUsuarioCreador)";
            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdReserva", pago.idReserva);
                    comando.Parameters.AddWithValue("@Concepto", pago.concepto);
                    comando.Parameters.AddWithValue("@FechaPago", pago.fechaPago);
                    comando.Parameters.AddWithValue("@Importe", pago.importe);
                    //comando.Parameters.AddWithValue("@Estado", pago.estado);
                    comando.Parameters.AddWithValue("@IdUsuarioCreador", pago.idUsuarioCreador);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ModificarConcepto(int idPago, string concepto)
        {
            string query = "UPDATE Pago SET Concepto = @Concepto WHERE IdPago = @IdPago";
            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion)) 
            {
                using (MySqlCommand command = new MySqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@Concepto", concepto);
                    command.Parameters.AddWithValue("@IdPago", idPago);

                    conexion.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AnularPago(int idPago, int idUsuarioAnulador)
        {
            string query = "UPDATE Pago SET Estado = 0, IdUsuarioAnulador = @IdUsuarioAnulador WHERE IdPago = @IdPago";
            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            { 
                using(MySqlCommand command = new MySqlCommand(query,conexion))
                {
                    command.Parameters.AddWithValue("@IdUsuarioAnulador", idUsuarioAnulador);
                    command.Parameters.AddWithValue("@IdPago", idPago);

                    conexion.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Pago> ObtenerPorReserva(int idReserva)
        {
            var lista = new List<Pago>();
            string query = "SELECT * FROM Pago WHERE IdReserva = @IdReserva";

            using (MySqlConnection connection = new MySqlConnection(cadenaDeConexion))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdReserva", idReserva);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Pago
                            {
                                idPago = reader.GetInt32("IdPago"),
                                idReserva = reader.GetInt32("IdReserva"),
                                concepto = reader.GetString("Concepto"),
                                fechaPago = reader.GetDateTime("FechaPago"),
                                importe = reader.GetDecimal("Importe"),
                                estado = reader.GetBoolean("Estado"),
                                idUsuarioCreador = reader.GetInt32("IdUsuarioCreador"),
                                idUsuarioAnulador = reader.IsDBNull(reader.GetOrdinal("IdUsuarioAnulador")) ? (int?)null : reader.GetInt32("IdUsuarioAnulador")
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<Pago> ObtenerTodos()
        {
            var pagos = new List<Pago>();
            string query = "SELECT * FROM Pago ORDER BY FechaPago DESC"; // Los ordenamos del más reciente al más antiguo

            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pagos.Add(new Pago
                            {
                                idPago = reader.GetInt32("IdPago"),
                                idReserva = reader.GetInt32("IdReserva"),
                                concepto = reader.GetString("Concepto"),
                                fechaPago = reader.GetDateTime("FechaPago"),
                                importe = reader.GetDecimal("Importe"),
                                estado = reader.GetBoolean("Estado")
                            });
                        }
                    }
                }
            }
            return pagos;
        }

        public List<Pago> ObtenerPaginados(
    string busqueda,
    bool? estado,
    int? idReserva,
    int pagina,
    int cantidadPorPagina)
        {
            List<Pago> lista = new List<Pago>();

            int desplazamiento =
                (pagina - 1) * cantidadPorPagina;

            string sql = @"
        SELECT
            p.IdPago,
            p.IdReserva,
            p.Concepto,
            p.FechaPago,
            p.Importe,
            p.Estado,
            p.IdUsuarioCreador,
            p.IdUsuarioAnulador
        FROM Pago p

        WHERE
        (
            CAST(p.IdPago AS CHAR) LIKE @Busqueda
            OR CAST(p.IdReserva AS CHAR) LIKE @Busqueda
            OR p.Concepto LIKE @Busqueda
            OR CAST(p.Importe AS CHAR) LIKE @Busqueda
        )

        AND (@Estado IS NULL OR p.Estado = @Estado)

        AND (@IdReserva IS NULL OR p.IdReserva = @IdReserva)

        ORDER BY p.FechaPago DESC

        LIMIT @CantidadPorPagina
        OFFSET @Desplazamiento";

            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                conexion.Open();

                using (MySqlCommand comando =
                       new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@Busqueda",
                        "%" + (busqueda ?? "") + "%");

                    comando.Parameters.AddWithValue(
                        "@Estado",
                        estado.HasValue
                            ? estado.Value
                            : DBNull.Value);

                    comando.Parameters.AddWithValue(
                        "@IdReserva",
                        idReserva.HasValue
                            ? idReserva.Value
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
                            Pago pago = new Pago
                            {
                                idPago = reader.GetInt32("IdPago"),
                                idReserva = reader.GetInt32("IdReserva"),
                                concepto = reader.GetString("Concepto"),
                                fechaPago = reader.GetDateTime("FechaPago"),
                                importe = reader.GetDecimal("Importe"),
                                estado = reader.GetBoolean("Estado"),

                                idUsuarioCreador =
                                    reader.GetInt32("IdUsuarioCreador"),

                                idUsuarioAnulador =
                                    reader.IsDBNull(
                                        reader.GetOrdinal("IdUsuarioAnulador"))
                                    ? (int?)null
                                    : reader.GetInt32("IdUsuarioAnulador")
                            };

                            lista.Add(pago);
                        }
                    }
                }
            }

            return lista;
        }

        public int ContarPagos(
    string busqueda,
    bool? estado,
    int? idReserva)
        {
            int cantidad = 0;

            string sql = @"
        SELECT COUNT(*)
        FROM Pago p

        WHERE
        (
            CAST(p.IdPago AS CHAR) LIKE @Busqueda
            OR CAST(p.IdReserva AS CHAR) LIKE @Busqueda
            OR p.Concepto LIKE @Busqueda
            OR CAST(p.Importe AS CHAR) LIKE @Busqueda
        )

        AND (@Estado IS NULL OR p.Estado = @Estado)

        AND (@IdReserva IS NULL OR p.IdReserva = @IdReserva)";

            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                conexion.Open();

                using (MySqlCommand comando =
                       new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@Busqueda",
                        "%" + (busqueda ?? "") + "%");

                    comando.Parameters.AddWithValue(
                        "@Estado",
                        estado.HasValue
                            ? estado.Value
                            : DBNull.Value);

                    comando.Parameters.AddWithValue(
                        "@IdReserva",
                        idReserva.HasValue
                            ? idReserva.Value
                            : DBNull.Value);

                    cantidad = Convert.ToInt32(
                        comando.ExecuteScalar());
                }
            }

            return cantidad;
        }

        public Pago ObtenerPorId(int idPago)
        {
            Pago pago = null;

            string query = "SELECT * FROM Pago WHERE IdPago = @IdPago";

            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdPago", idPago);

                    conexion.Open();

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pago = new Pago
                            {
                                idPago = reader.GetInt32("IdPago"),
                                idReserva = reader.GetInt32("IdReserva"),
                                concepto = reader.GetString("Concepto"),
                                fechaPago = reader.GetDateTime("FechaPago"),
                                importe = reader.GetDecimal("Importe"),
                                estado = reader.GetBoolean("Estado"),

                                idUsuarioCreador = reader.GetInt32("IdUsuarioCreador"),

                                idUsuarioAnulador = reader.IsDBNull(
                                    reader.GetOrdinal("IdUsuarioAnulador"))
                                    ? (int?)null
                                    : reader.GetInt32("IdUsuarioAnulador")
                            };
                        }
                    }
                }
            }

            return pago;
        }
    }
}
