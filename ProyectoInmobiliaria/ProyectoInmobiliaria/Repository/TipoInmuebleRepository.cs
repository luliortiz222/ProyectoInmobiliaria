using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;
namespace ProyectoInmobiliaria.Repository
{
    public class TipoInmuebleRepository
    {
        private readonly string connectionString;

        public TipoInmuebleRepository(string cadenaConexion)
        {
            connectionString = cadenaConexion;
        }

        // Obtener todos los tipos de inmuebles
        public List<TipoInmueble> ObtenerTodos()
        {
            List<TipoInmueble> lista = new List<TipoInmueble>();

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = "SELECT IdTipoInmueble, Nombre FROM TipoInmueble";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipoInmueble tipo = new TipoInmueble
                            {
                                IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                Nombre = reader.GetString("Nombre")
                            };

                            lista.Add(tipo);
                        }
                    }
                }
            }

            return lista;
        }

        public List<TipoInmueble> ObtenerPaginados(string busqueda, int pagina, int cantidadPorPagina)
        {
            List<TipoInmueble> lista = new List<TipoInmueble>();

            int desplazamiento = (pagina - 1) * cantidadPorPagina;

            string sql = @"
        SELECT IdTipoInmueble, Nombre
        FROM TipoInmueble
        WHERE Nombre LIKE @Busqueda
        ORDER BY IdTipoInmueble
        LIMIT @CantidadPorPagina OFFSET @Desplazamiento";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Busqueda", "%" + (busqueda ?? "") + "%");
                    comando.Parameters.AddWithValue("@CantidadPorPagina", cantidadPorPagina);
                    comando.Parameters.AddWithValue("@Desplazamiento", desplazamiento);

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipoInmueble tipo = new TipoInmueble
                            {
                                IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                Nombre = reader.GetString("Nombre")
                            };

                            lista.Add(tipo);
                        }
                    }
                }
            }

            return lista;
        }

        public int ContarTiposInmueble(string busqueda)
        {
            int cantidad = 0;

            string sql = @"
        SELECT COUNT(*)
        FROM TipoInmueble
        WHERE Nombre LIKE @Busqueda";

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@Busqueda", "%" + (busqueda ?? "") + "%");

                    cantidad = Convert.ToInt32(comando.ExecuteScalar());
                }
            }

            return cantidad;
        }

        // Buscar un tipo de inmueble por ID
        public TipoInmueble ObtenerPorId(int id)
        {
            TipoInmueble tipo = null;

            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = "SELECT IdTipoInmueble, Nombre FROM TipoInmueble WHERE IdTipoInmueble = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tipo = new TipoInmueble
                            {
                                IdTipoInmueble = reader.GetInt32("IdTipoInmueble"),
                                Nombre = reader.GetString("Nombre")
                            };
                        }
                    }
                }
            }

            return tipo;
        }

        // Guardar un nuevo tipo de inmueble
        public void Guardar(TipoInmueble tipo)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = "INSERT INTO TipoInmueble (Nombre) VALUES (@nombre)";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", tipo.Nombre);

                    comando.ExecuteNonQuery();
                }
            }
        }

        // Editar un tipo de inmueble
        public void Editar(TipoInmueble tipo)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = @"UPDATE TipoInmueble
                               SET Nombre = @nombre
                               WHERE IdTipoInmueble = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", tipo.Nombre);
                    comando.Parameters.AddWithValue("@id", tipo.IdTipoInmueble);

                    comando.ExecuteNonQuery();
                }
            }
        }

        // Eliminar un tipo de inmueble
        public void Eliminar(int id)
        {
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                string sql = "DELETE FROM TipoInmueble WHERE IdTipoInmueble = @id";

                using (MySqlCommand comando = new MySqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
