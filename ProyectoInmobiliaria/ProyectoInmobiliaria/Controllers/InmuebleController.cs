using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;
using System;

namespace ProyectoInmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly InmuebleRepository _inmuebleRepository;
        private readonly PropietarioRepository _propietarioRepo;
        private readonly TipoInmuebleRepository _tipoRepo;

        public InmuebleController(
            InmuebleRepository inmuebleRepository,
            PropietarioRepository propietarioRepo,
            TipoInmuebleRepository tipoRepo)
        {
            _inmuebleRepository = inmuebleRepository;
            _propietarioRepo = propietarioRepo;
            _tipoRepo = tipoRepo;
        }

        // GET: /Inmueble
        public IActionResult Index()
        {
            var inmuebles = _inmuebleRepository.ObtenerTodos();
            return View(inmuebles);
        }

        // GET: /Inmueble/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Propietarios = _propietarioRepo.obtenerTodos();
            ViewBag.TiposInmueble = _tipoRepo.ObtenerTodos();

            return View();
        }

        // POST: /Inmueble/Create
        [HttpPost]
        public IActionResult Create(Inmueble inmueble)
        {
            _inmuebleRepository.Guardar(inmueble);

            return RedirectToAction("Index");
        }

        // GET: /Inmueble/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Inmueble inmueble = _inmuebleRepository.ObtenerPorId(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            ViewBag.Propietarios = _propietarioRepo.obtenerTodos();
            ViewBag.TiposInmueble = _tipoRepo.ObtenerTodos();

            return View(inmueble);
        }

        // POST: /Inmueble/Edit
        [HttpPost]
        public IActionResult Edit(Inmueble inmueble)
        {
            _inmuebleRepository.Actualizar(inmueble);

            return RedirectToAction("Index");
        }

        // GET: /Inmueble/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            var inmueble = _inmuebleRepository.ObtenerPorId(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            return View(inmueble);
        }

        // GET: /Inmueble/Borrar/5
        [HttpGet]
        public IActionResult Borrar(int id)
        {
            var inmueble = _inmuebleRepository.ObtenerPorId(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            return View(inmueble);
        }

        // POST: /Inmueble/BorrarConfirmado
        [HttpPost]
        public IActionResult BorrarConfirmado(int IdInmueble)
        {
            _inmuebleRepository.Eliminar(IdInmueble);

            return RedirectToAction("Index");
        }
    }
}