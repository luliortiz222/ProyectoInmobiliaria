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
            string query = "INSERT INTO Pago (IdReserva, Concepto, FechaPago, Importe, Estado, IdUsuarioCreador) VALUES (@idReserva, @concepto, @fechaPago, @importe, @estado, @idUsuarioCreador)";
            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdReserva", pago.idReserva);
                    comando.Parameters.AddWithValue("@Concepto", pago.concepto);
                    comando.Parameters.AddWithValue("@FechaPago", pago.fechaPago);
                    comando.Parameters.AddWithValue("@Importe", pago.importe);
                    comando.Parameters.AddWithValue("@Estado", pago.estado);
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
                                estado = reader.GetBoolean("Estado")
                            };
                        }
                    }
                }
            }
            return pago;
        }
    }
}
