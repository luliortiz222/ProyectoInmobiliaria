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

        public IActionResult Index(
    string busqueda,
    int? idInquilino,
    int? idInmueble,
    int pagina = 1)
        {
            int cantidadPorPagina = 10;

            int totalReservas = _reservaRepository.ContarReservas(
                busqueda,
                idInquilino,
                idInmueble);

            int totalPaginas = (int)Math.Ceiling(
                (double)totalReservas / cantidadPorPagina);

            if (totalPaginas > 0 && pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }

            if (pagina < 1)
            {
                pagina = 1;
            }

            var lista = _reservaRepository.ObtenerPaginados(
                busqueda,
                idInquilino,
                idInmueble,
                pagina,
                cantidadPorPagina);

            // Datos para los selects de búsqueda
            ViewBag.Inquilinos = _inquilinoRepository.ObtenerTodos();
            ViewBag.Inmuebles = _inmuebleRepository.ObtenerTodos();

            ViewBag.Busqueda = busqueda;
            ViewBag.IdInquilino = idInquilino;
            ViewBag.IdInmueble = idInmueble;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

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
            string idString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            int idUsuarioCreador = int.Parse(idString);

            // Obtener el inmueble seleccionado
            Inmueble inmueble =
                _inmuebleRepository.ObtenerPorId(reserva.IdInmueble);

            if (inmueble == null)
            {
                ModelState.AddModelError(
                    "",
                    "No se encontró el inmueble seleccionado.");

                ViewBag.Inquilinos =
                    _inquilinoRepository.ObtenerTodos();

                ViewBag.Inmuebles =
                    _inmuebleRepository.ObtenerTodos();

                return View(reserva);
            }


            // El monto por día sale del inmueble
            reserva.MontoPorDia =
                inmueble.PrecioPorDia;


            reserva.IdUsuarioCreador =
                idUsuarioCreador;


            if (inmueble.PorcentajeReserva < 0 ||
                inmueble.PorcentajeReserva > 100)
            {
                ModelState.AddModelError(
                    "",
                    "El porcentaje de reserva del inmueble no es válido.");

                ViewBag.Inquilinos =
                    _inquilinoRepository.ObtenerTodos();

                ViewBag.Inmuebles =
                    _inmuebleRepository.ObtenerTodos();

                return View(reserva);
            }


            // Guardar reserva + pago inicial
            int idReserva =
                _reservaRepository.Guardar(
                    reserva,
                    inmueble.PorcentajeReserva);


            if (idReserva <= 0)
            {
                ModelState.AddModelError(
                    "",
                    "No se puede realizar la reserva. Verifique las fechas o si el inmueble ya está reservado.");

                ViewBag.Inquilinos =
                    _inquilinoRepository.ObtenerTodos();

                ViewBag.Inmuebles =
                    _inmuebleRepository.ObtenerTodos();

                return View(reserva);
            }


            // Calcular importe inicial para mostrar confirmación
            int dias =
                (reserva.FechaHasta.Date -
                 reserva.FechaDesde.Date).Days;

            decimal totalAlquiler =
                dias * reserva.MontoPorDia;

            decimal pagoInicial =
                Math.Round(
                    totalAlquiler *
                    inmueble.PorcentajeReserva / 100m,
                    2);


            if (pagoInicial > 0)
            {
                TempData["Mensaje"] =
                    $"Reserva creada correctamente. " +
                    $"Se registró un pago inicial de ${pagoInicial:N2} " +
                    $"({inmueble.PorcentajeReserva:N2}% del alquiler).";
            }
            else
            {
                TempData["Mensaje"] =
                    "Reserva creada correctamente. " +
                    "El inmueble no requiere pago inicial.";
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
        public IActionResult Finalizar(int id)
        {
            var reserva = _reservaRepository.ObtenerPorId(id);

            if (reserva == null)
                return NotFound();

            if (reserva.IdUsuarioFinalizador != null)
            {
                return View("Finalizar", reserva);
            }

            return View(reserva);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult FinalizarConfirmado(
    int idReserva,
    DateTime fechaTerminacion)
        {
            string idString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            int idUsuarioFinalizador = int.Parse(idString);

            bool finalizado = _reservaRepository.FinalizarConMulta(
                idReserva,
                fechaTerminacion,
                idUsuarioFinalizador);

            if (!finalizado)
            {
                TempData["Error"] =
                    "No se pudo finalizar la reserva. Verifique la fecha o si la reserva ya fue finalizada.";

                return RedirectToAction(
                    "Finalizar",
                    new { id = idReserva });
            }

            Reserva reserva = _reservaRepository.ObtenerPorId(idReserva);

            decimal multa = _reservaRepository.CalcularMulta(
                reserva,
                fechaTerminacion);

            if (multa > 0)
            {
                TempData["Mensaje"] =
                    $"La reserva fue finalizada correctamente. " +
                    $"La multa de ${multa:N2} fue registrada como pago.";
            }
            else
            {
                TempData["Mensaje"] =
                    "La reserva fue finalizada correctamente. " +
                    "No correspondía multa porque finalizó en la fecha original.";
            }

            return RedirectToAction(
                "Details",
                new { id = idReserva });
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
            bool eliminado = _reservaRepository.Eliminar(id);

            if (!eliminado)
            {
                TempData["Error"] = "No se puede eliminar la reserva porque tiene pagos asociados.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Renovar(int id)
        {
            var reserva = _reservaRepository.ObtenerPorId(id);

            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.IdUsuarioFinalizador != null)
            {
                TempData["Error"] =
                    "No se puede renovar una reserva que ya fue finalizada.";

                return RedirectToAction(
                    "Details",
                    new { id = id });
            }

            return View(reserva);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Renovar(
    int idReserva,
    DateTime nuevaFechaDesde,
    DateTime nuevaFechaHasta,
    decimal nuevoMontoPorDia)
        {
            string idString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            int idUsuarioCreador = int.Parse(idString);

            // Obtener la reserva original
            Reserva reservaOriginal = _reservaRepository.ObtenerPorId(idReserva);

            if (reservaOriginal == null)
            {
                TempData["Error"] =
                    "No se encontró la reserva original.";

                return RedirectToAction("Index");
            }

            // El nuevo monto debe ser mayor a cero
            if (nuevoMontoPorDia <= 0)
            {
                TempData["Error"] =
                    "El nuevo monto por día debe ser mayor a cero.";

                return RedirectToAction(
                    "Renovar",
                    new { id = idReserva });
            }

            // El nuevo monto debe ser diferente al original
            if (nuevoMontoPorDia == reservaOriginal.MontoPorDia)
            {
                TempData["Error"] =
                    "El nuevo monto por día debe ser diferente al monto de la reserva original.";

                return RedirectToAction(
                    "Renovar",
                    new { id = idReserva });
            }

            bool renovada = _reservaRepository.Renovar(
                idReserva,
                nuevaFechaDesde,
                nuevaFechaHasta,
                nuevoMontoPorDia,
                idUsuarioCreador);

            if (!renovada)
            {
                TempData["Error"] =
                    "No se pudo renovar la reserva. Verifique las fechas, el monto o la disponibilidad del inmueble.";

                return RedirectToAction(
                    "Renovar",
                    new { id = idReserva });
            }

            TempData["Mensaje"] =
                "La reserva fue renovada correctamente. Se creó una nueva reserva sin modificar la original.";

            return RedirectToAction(
                "Details",
                new { id = idReserva });
        }
    }
}
