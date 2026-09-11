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

        [HttpGet]
        public IActionResult PorEstado(bool? estado)
        {
            List<Inmueble> inmuebles;

            if (estado == null)
            {
                inmuebles = _inmuebleRepository.ObtenerTodos();
            }
            else
            {
                inmuebles = _inmuebleRepository.ObtenerPorEstado(estado.Value);
            }

            ViewBag.Estado = estado;

            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult PorPropietario(int? idPropietario)
        {
            ViewBag.Propietarios = _propietarioRepo.obtenerTodos();

            if (idPropietario == null)
            {
                return View(new List<Inmueble>());
            }

            var inmuebles = _inmuebleRepository.ObtenerPorPropietario(idPropietario.Value);

            ViewBag.IdPropietario = idPropietario.Value;

            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult MasReservados()
        {
            var inmuebles = _inmuebleRepository.ObtenerMasReservados();

            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult SinReservas(int dias)
        {
            if (dias <= 0)
            {
                dias = 30;
            }

            var inmuebles = _inmuebleRepository.ObtenerSinReservas(dias);

            ViewBag.Dias = dias;

            return View(inmuebles);
        }

        [HttpGet]
        public IActionResult DisponiblesEntreFechas(
    DateTime? fechaDesde,
    DateTime? fechaHasta)
        {
            if (!fechaDesde.HasValue || !fechaHasta.HasValue)
            {
                ViewBag.FechaDesde = "";
                ViewBag.FechaHasta = "";

                return View(new List<Inmueble>());
            }

            if (fechaDesde.Value >= fechaHasta.Value)
            {
                ViewBag.Mensaje = "La fecha desde debe ser anterior a la fecha hasta.";
                ViewBag.FechaDesde = fechaDesde.Value.ToString("yyyy-MM-dd");
                ViewBag.FechaHasta = fechaHasta.Value.ToString("yyyy-MM-dd");

                return View(new List<Inmueble>());
            }

            var inmuebles = _inmuebleRepository.ObtenerDisponiblesEntreFechas(
                fechaDesde.Value,
                fechaHasta.Value);

            ViewBag.FechaDesde = fechaDesde.Value.ToString("yyyy-MM-dd");
            ViewBag.FechaHasta = fechaHasta.Value.ToString("yyyy-MM-dd");

            return View(inmuebles);
        }

        // GET: /Inmueble/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Propietarios = _propietarioRepo.obtenerTodos();
            ViewBag.TipoInmueble = _tipoRepo.ObtenerTodos();

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
        public IActionResult Delete(int id)
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