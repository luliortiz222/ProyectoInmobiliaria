using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;



namespace ProyectoInmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // GET: Usuario
        public IActionResult Index()
        {
            var usuarios = _usuarioRepository.ObtenerTodos();

            return View(usuarios);
        }



        // GET: /Usuarios/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Usuarios/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Buscamos al usuario por su email usando el repositorio que armamos antes
            var usuario = _usuarioRepository.ObtenerPorEmail(email);

            // 2. Verificamos si existe y si la clave coincide
            if (usuario == null || usuario.Password != password)
            {
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            // 3. Creamos la "Credencial" (Claims)
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Rol), // "Administrador" o "Empleado"
                    new Claim("Avatar", usuario.Avatar ?? "")
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // 4. Iniciamos sesión oficialmente (crea la cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));


            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

            // AGREGA ESTA LÍNEA:
            TempData["MensajeBienvenida"] = $"¡Hola {usuario.Nombre}! Has iniciado sesión correctamente.";

            // 5. Lo mandamos a la página principal
            return RedirectToAction("Index", "Inmueble");
        }

        // GET: /Usuarios/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }






        // GET: Usuario/Create
        [HttpGet]
        public IActionResult Create()
        {
            string carpetaAvatares = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "avatars"
            );

            var avatares = new List<string>();

            if (Directory.Exists(carpetaAvatares))
            {
                string[] archivos = Directory.GetFiles(carpetaAvatares);

                foreach (string archivo in archivos)
                {
                    avatares.Add(Path.GetFileName(archivo));
                }
            }

            ViewBag.Avatares = avatares;

            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            _usuarioRepository.Guardar(usuario);

            return RedirectToAction("Login");
        }
        // GET: Usuario/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            string carpetaAvatares = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "avatars"
            );

            var avatares = new List<string>();

            if (Directory.Exists(carpetaAvatares))
            {
                string[] archivos = Directory.GetFiles(carpetaAvatares);

                foreach (string archivo in archivos)
                {
                    avatares.Add(Path.GetFileName(archivo));
                }
            }

            ViewBag.Avatares = avatares;

            return View(usuario);
        }

        // POST: Usuario/Edit
        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            _usuarioRepository.Actualizar(usuario);

            return RedirectToAction("Index");
        }

        // GET: Usuario/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _usuarioRepository.Eliminar(id);

            return RedirectToAction("Index");
        }

        private bool EsAdministrador()
        {
            return HttpContext.Session.GetString("Rol") == "Administrador";
        }
    }
}
