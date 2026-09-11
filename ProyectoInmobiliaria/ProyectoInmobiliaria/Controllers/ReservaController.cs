using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;
using System.Security.Claims;

namespace ProyectoInmobiliaria.Controllers
{
    [Authorize]
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
        public IActionResult Vigentes()
        {
            var reservas = _reservaRepository.ObtenerVigentes();

            return View(reservas);
        }

        [HttpGet]
        public IActionResult TerminanEn(int dias)
        {
            if (dias < 0)
            {
                dias = 7;
            }

            var reservas = _reservaRepository.ObtenerQueTerminanEnDias(dias);

            ViewBag.Dias = dias;

            return View(reservas);
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
            string idString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lo convertimos a número y se lo asignamos a la reserva
            reserva.IdUsuarioCreador = int.Parse(idString);
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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            _reservaRepository.Eliminar(id);

            return RedirectToAction("Index");
        }
    }
}
