using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies; 
// librería de MySQL
using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.Controllers;
using ProyectoInmobiliaria.Repository;
using System;
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

                    // Configuración de la Autenticación
                    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.LoginPath = "/Usuario/Login"; // A donde te manda si no estás logueado
                            options.AccessDeniedPath = "/Home/Index"; // A donde te manda si no tienes permiso
                        });

                    builder.Services.AddSingleton(new PropietarioRepository(cadenaConexion));
                    builder.Services.AddSingleton(new InquilinoRepository(cadenaConexion));
                    builder.Services.AddSingleton(new TipoInmuebleRepository(cadenaConexion));
                    builder.Services.AddSingleton(new InmuebleRepository(cadenaConexion));
                    builder.Services.AddSingleton(new ReservaRepository(cadenaConexion));
                    builder.Services.AddSingleton(new UsuarioRepository(cadenaConexion));
                    builder.Services.AddSingleton(new PagoRepository(cadenaConexion));
                    builder.Services.AddControllersWithViews();

                    builder.Services.AddSession();

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
                    app.UseStaticFiles();
                    app.UseSession();
                    app.UseCors("PermitirTodo");
                    app.UseAuthentication(); // ESTA LÍNEA ES NUEVA (Debe ir antes que Authorization)
                    app.UseAuthorization();
                    app.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Usuario}/{action=Login}/{id?}");
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