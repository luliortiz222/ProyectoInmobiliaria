using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;
using System.Linq;
using System.IO;
using System.Collections.Generic;

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

            return RedirectToAction("Index");
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
