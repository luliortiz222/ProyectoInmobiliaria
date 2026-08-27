using System;
// librería de MySQL
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace ProyectoInmobiliaria

{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string cadenaConexion = "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=admin;";

            
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                { 
                    conexion.Open();
                    Console.WriteLine("¡Conexión a MySQL establecida con éxito!");

                    builder.Services.AddSingleton(new PropietarioRepository(cadenaConexion));
                    builder.Services.AddSingleton(new InquilinoRepository(cadenaConexion));
                    builder.Services.AddControllersWithViews();

                    builder.Services.AddCors(options =>
                    {
                        options.AddPolicy("PermitirTodo", policy =>
                        {
                            policy.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                        });
                    });

                    var app = builder.Build();
                    app.UseCors("PermitirTodo");
                    app.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Propietarios}/{action=Index}/{id?}");
                    app.Run();
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error al conectar: " + ex.Message);
                }
            }


        }
    }
}