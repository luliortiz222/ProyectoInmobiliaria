using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using System.IO; 
using System;



namespace ProyectoInmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IWebHostEnvironment _entorno;

        public UsuarioController(UsuarioRepository usuarioRepository, IWebHostEnvironment entorno)
        {
            _usuarioRepository = usuarioRepository;
            _entorno = entorno;
        }

        // GET: Usuario
        [HttpGet]
        [Authorize]
        public IActionResult Index(string busqueda, int pagina = 1)
        {
            //ve todos los usuarios con búsqueda y paginación
            if (User.IsInRole("Administrador"))
            {
                int cantidadPorPagina = 10;

                int totalUsuarios = _usuarioRepository.ContarUsuarios(busqueda);

                int totalPaginas = (int)Math.Ceiling(
                    (double)totalUsuarios / cantidadPorPagina);

                if (totalPaginas > 0 && pagina > totalPaginas)
                {
                    pagina = totalPaginas;
                }

                if (pagina < 1)
                {
                    pagina = 1;
                }

                var usuarios = _usuarioRepository.ObtenerPaginados(
                    busqueda,
                    pagina,
                    cantidadPorPagina);

                ViewBag.Busqueda = busqueda;
                ViewBag.PaginaActual = pagina;
                ViewBag.TotalPaginas = totalPaginas;

                return View(usuarios);
            }

            //solamente puede ver su propio usuario
            string idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idString, out int idUsuarioLogueado))
            {
                return Forbid();
            }

            var usuarioPropio = _usuarioRepository.ObtenerPorId(idUsuarioLogueado);

            if (usuarioPropio == null)
            {
                return NotFound();
            }

            ViewBag.Busqueda = null;
            ViewBag.PaginaActual = 1;
            ViewBag.TotalPaginas = 1;

            return View(new List<Usuario> { usuarioPropio });
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

            TempData["MensajeBienvenida"] = $"¡Hola {usuario.Nombre}! Has iniciado sesión correctamente.";

            // 5. Lo mandamos a la página principal
            return RedirectToAction("Index", "Home");
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

            try
            {
                if (usuario.ArchivoImagen != null && usuario.ArchivoImagen.Length > 0)
                {
                    string carpetaAvatares = Path.Combine(_entorno.WebRootPath, "avatars");
                    if (!Directory.Exists(carpetaAvatares))
                    {
                        Directory.CreateDirectory(carpetaAvatares);
                    }
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(usuario.ArchivoImagen.FileName);
                    string rutaArchivo = Path.Combine(carpetaAvatares, nombreArchivo);
                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        usuario.ArchivoImagen.CopyTo(stream);
                    }
                    usuario.Avatar = "/avatars/" + nombreArchivo;
                }
                else
                {
                    usuario.Avatar = " ";

                }
                _usuarioRepository.Guardar(usuario);

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                
                return View(usuario);
            }
            
        }
        // GET: Usuario/Edit/5
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
                return NotFound();

            // El empleado solamente puede editarse a sí mismo
            if (!User.IsInRole("Administrador"))
            {
                string idUsuarioLogueado =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                int idLogueado = int.Parse(idUsuarioLogueado);

                if (id != idLogueado)
                    return Forbid();
            }

            // Nunca mandar la contraseña a la vista
            usuario.Password = null;

            // Cargar avatares
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

            // MUY IMPORTANTE
            ViewBag.Avatares = avatares;

            return View(usuario);
        }



        // POST: Usuario/Edit
        [HttpPost]
        [Authorize]
        public IActionResult Edit(Usuario usuario)
        {
            var usuarioExistente =
                _usuarioRepository.ObtenerPorId(usuario.IdUsuario);

            if (usuarioExistente == null)
                return NotFound();

            // El empleado solo puede editarse a sí mismo
            if (!User.IsInRole("Administrador"))
            {
                string idUsuarioLogueado =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                int idLogueado = int.Parse(idUsuarioLogueado);

                if (usuario.IdUsuario != idLogueado)
                    return Forbid();

                // El empleado NO puede cambiar su rol
                usuario.Rol = usuarioExistente.Rol;
            }

            if (!ModelState.IsValid)
            {
                // Volver a cargar avatares
                string carpetaAvatares = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "avatars"
                );

                var avatares = new List<string>();

                if (Directory.Exists(carpetaAvatares))
                {
                    foreach (string archivo in Directory.GetFiles(carpetaAvatares))
                    {
                        avatares.Add(Path.GetFileName(archivo));
                    }
                }

                ViewBag.Avatares = avatares;

                return View(usuario);
            }

            _usuarioRepository.Actualizar(usuario);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            var usuario = _usuarioRepository.ObtenerPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Administrador"))
            {
                string idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(idString, out int idUsuarioLogueado))
                {
                    return Forbid();
                }

                if (id != idUsuarioLogueado)
                {
                    return Forbid();
                }
            }

            usuario.Password = null;

            return View(usuario);
        }


        // GET: Usuario/Delete/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            _usuarioRepository.Eliminar(id);

            return RedirectToAction("Index");
        }
    }
}
