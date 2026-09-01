using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;

namespace ProyectoInmobiliaria.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaRepository _reservaRepository;
        private readonly InquilinoRepository _inquilinoRepository;
        private readonly InmuebleRepository _inmuebleRepository;

        public ReservasController(
            ReservaRepository reservaRepository,
            InquilinoRepository inquilinoRepository,
            InmuebleRepository inmuebleRepository)
        {
            _reservaRepository = reservaRepository;
            _inquilinoRepository = inquilinoRepository;
            _inmuebleRepository = inmuebleRepository;
        }

        public IActionResult Index()
        {
            var lista = _reservaRepository.ObtenerTodos();

            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Inquilinos = _inquilinoRepository.ObtenerTodos();
            ViewBag.Inmuebles = _inmuebleRepository.ObtenerTodos();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Reserva reserva)
        {
            bool guardado = _reservaRepository.Guardar(reserva);

            if (!guardado)
            {
                ModelState.AddModelError(
                    "",
                    "No se puede realizar la reserva. Verifique si las fechas o el inmueble ya está reservado."
                );

                ViewBag.Inquilinos = _inquilinoRepository.ObtenerTodos();
                ViewBag.Inmuebles = _inmuebleRepository.ObtenerTodos();

                return View(reserva);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var reserva = _reservaRepository.ObtenerPorId(id);

            if (reserva == null)
            {
                return NotFound();
            }

            ViewBag.Inquilinos = _inquilinoRepository.ObtenerTodos();
            ViewBag.Inmuebles = _inmuebleRepository.ObtenerTodos();

            return View(reserva);
        }

        [HttpPost]
        public IActionResult Edit(Reserva reserva)
        {
            bool editado = _reservaRepository.Editar(reserva);

            if (!editado)
            {
                ModelState.AddModelError(
                    "",
                    "No se puede modificar la reserva. Verifique si las fechas o el inmueble ya está reservado."
                );

                ViewBag.Inquilinos = _inquilinoRepository.ObtenerTodos();
                ViewBag.Inmuebles = _inmuebleRepository.ObtenerTodos();

                return View(reserva);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var reserva = _reservaRepository.ObtenerPorId(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var reserva = _reservaRepository.ObtenerPorId(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _reservaRepository.Eliminar(id);

            return RedirectToAction("Index");
        }
    }
}
