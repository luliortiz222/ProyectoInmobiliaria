using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;

namespace ProyectoInmobiliaria.Repository
{
    public class UsuarioRepository
    {
        private string _cadenaDeConexion;

        public UsuarioRepository(string cadenaDeConexion)
        {
            _cadenaDeConexion = cadenaDeConexion;
        }

        // Guardar un usuario 
        public void Guardar(Usuario usuario)
        {
            string query = @"INSERT INTO Usuario
                            (Email, Password, Nombre, Apellido, Avatar, Rol)
                            VALUES
                            (@Email, @Password, @Nombre, @Apellido, @Avatar, @Rol)";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Email", usuario.Email);
                    comando.Parameters.AddWithValue("@Password", usuario.Password);
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@Avatar", usuario.Avatar);
                    comando.Parameters.AddWithValue("@Rol", usuario.Rol);

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();

                        Console.WriteLine("Usuario guardado con éxito");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al guardar usuario: " + ex.Message);
                    }
                }
            }
        }

        // Obtener todos los usuarios
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string query = @"SELECT
                            IdUsuario,
                            Email,
                            Password,
                            Nombre,
                            Apellido,
                            Avatar,
                            Rol
                            FROM Usuario";

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
                                Usuario usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Email = reader.GetString("Email"),
                                    Password = reader.GetString("Password"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),

                                    Avatar = reader.IsDBNull(
                                        reader.GetOrdinal("Avatar"))
                                        ? ""
                                        : reader.GetString("Avatar"),

                                    Rol = reader.GetString("Rol")
                                };

                                usuarios.Add(usuario);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener usuarios: " + ex.Message);
                    }
                }
            }

            return usuarios;
        }

        // Obtener usuarios por id 
        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario usuario = null;

            string query = @"SELECT
                            IdUsuario,
                            Email,
                            Password,
                            Nombre,
                            Apellido,
                            Avatar,
                            Rol
                            FROM Usuario
                            WHERE IdUsuario = @IdUsuario";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    try
                    {
                        conexion.Open();

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Email = reader.GetString("Email"),
                                    Password = reader.GetString("Password"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),

                                    Avatar = reader.IsDBNull(
                                        reader.GetOrdinal("Avatar"))
                                        ? ""
                                        : reader.GetString("Avatar"),

                                    Rol = reader.GetString("Rol")
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener usuario por ID: " + ex.Message);
                    }
                }
            }

            return usuario;
        }

        // Obtener usuarios por email 
        public Usuario ObtenerPorEmail(string email)
        {
            Usuario usuario = null;

            string query = @"SELECT
                            IdUsuario,
                            Email,
                            Password,
                            Nombre,
                            Apellido,
                            Avatar,
                            Rol
                            FROM Usuario
                            WHERE Email = @Email";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        conexion.Open();

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Email = reader.GetString("Email"),
                                    Password = reader.GetString("Password"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),

                                    Avatar = reader.IsDBNull(
                                        reader.GetOrdinal("Avatar"))
                                        ? ""
                                        : reader.GetString("Avatar"),

                                    Rol = reader.GetString("Rol")
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al obtener usuario por email: " + ex.Message);
                    }
                }
            }

            return usuario;
        }

        // Obtener usuario para login por email y password
        public Usuario ObtenerPorEmailYPassword(string email, string password)
        {
            Usuario usuario = null;

            string query = @"SELECT
                            IdUsuario,
                            Email,
                            Password,
                            Nombre,
                            Apellido,
                            Avatar,
                            Rol
                            FROM Usuario
                            WHERE Email = @Email
                            AND Password = @Password";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Email", email);
                    comando.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conexion.Open();

                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = reader.GetInt32("IdUsuario"),
                                    Email = reader.GetString("Email"),
                                    Password = reader.GetString("Password"),
                                    Nombre = reader.GetString("Nombre"),
                                    Apellido = reader.GetString("Apellido"),

                                    Avatar = reader.IsDBNull(
                                        reader.GetOrdinal("Avatar"))
                                        ? ""
                                        : reader.GetString("Avatar"),

                                    Rol = reader.GetString("Rol")
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al iniciar sesión: " + ex.Message);
                    }
                }
            }

            return usuario;
        }

        // Actualizar usuario 
        public void Actualizar(Usuario usuario)
        {
            string query = @"UPDATE Usuario
                            SET Email = @Email,
                                Password = @Password,
                                Nombre = @Nombre,
                                Apellido = @Apellido,
                                Avatar = @Avatar,
                                Rol = @Rol
                            WHERE IdUsuario = @IdUsuario";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@Email", usuario.Email);
                    comando.Parameters.AddWithValue("@Password", usuario.Password);
                    comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                    comando.Parameters.AddWithValue("@Avatar", usuario.Avatar);
                    comando.Parameters.AddWithValue("@Rol", usuario.Rol);
                    comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                    try
                    {
                        conexion.Open();

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Usuario actualizado con éxito");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el usuario");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al actualizar usuario: " + ex.Message);
                    }
                }
            }
        }

        // Eliminar usuario 
        public void Eliminar(int idUsuario)
        {
            string query = @"DELETE FROM Usuario
                            WHERE IdUsuario = @IdUsuario";

            using (MySqlConnection conexion = new MySqlConnection(_cadenaDeConexion))
            {
                using (MySqlCommand comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    try
                    {
                        conexion.Open();

                        int filasAfectadas = comando.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            Console.WriteLine("Usuario eliminado con éxito");
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el usuario");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                    }
                }
            }
        }
    }
}
