using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;
using System;

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
            string query = "INSERT INTO Pago (idReserva, concepto, fechaPago, importe, estado, idUsuarioCreador) VALUES (@idReserva, @concepto, @fechaPago, @importe, @estado, @idUsuarioCreador)";
            using (MySqlConnection conexion = new MySqlConnection(cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdReserva", pago.IdReserva);
                    comando.Parameters.AddWithValue("@Concepto", pago.Concepto);
                    comando.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                    comando.Parameters.AddWithValue("@Importe", pago.Importe);
                    comando.Parameters.AddWithValue("@IdUsuarioCreador", pago.IdUsuarioCreador);

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
                    comando.Parameters.AddWithValue("@Concepto", nuevoConcepto);
                    comando.Parameters.AddWithValue("@IdPago", idPago);

                    conexion.Open();
                    comando.ExecuteNonQuery();
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
                    comando.Parameters.AddWithValue("@IdUsuarioAnulador", idUsuarioAnulador);
                    comando.Parameters.AddWithValue("@IdPago", idPago);

                    conexion.Open();
                    comando.ExecuteNonQuery();
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
                                IdPago = reader.GetInt32("IdPago"),
                                IdReserva = reader.GetInt32("IdReserva"),
                                Concepto = reader.GetString("Concepto"),
                                FechaPago = reader.GetDateTime("FechaPago"),
                                Importe = reader.GetDecimal("Importe"),
                                Estado = reader.GetBoolean("Estado"),
                                IdUsuarioCreador = reader.GetInt32("IdUsuarioCreador"),
                                IdUsuarioAnulador = reader.IsDBNull(reader.GetOrdinal("IdUsuarioAnulador")) ? (int?)null : reader.GetInt32("IdUsuarioAnulador")
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
