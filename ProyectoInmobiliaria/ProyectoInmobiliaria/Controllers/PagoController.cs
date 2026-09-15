using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;
using System;
using System.Security.Claims;


namespace ProyectoInmobiliaria.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly PagoRepository _pagoRepository;
        public PagoController(PagoRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }

        // GET: /Pagos
        [HttpGet]
        public IActionResult Index(string busqueda, bool? estado, int pagina = 1)
        {
            int cantidadPorPagina = 10;

            int totalPagos = _pagoRepository.ContarPagos(busqueda, estado);

            int totalPaginas = (int)Math.Ceiling(
                (double)totalPagos / cantidadPorPagina);

            if (totalPaginas > 0 && pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }

            if (pagina < 1)
            {
                pagina = 1;
            }

            var pagos = _pagoRepository.ObtenerPaginados(
                busqueda,
                estado,
                pagina,
                cantidadPorPagina);

            ViewBag.Busqueda = busqueda;
            ViewBag.Estado = estado;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(pagos);
        }


        [HttpGet]
        public IActionResult PorReserva(int id)
        {
            ViewBag.idReserva = id;
            var lista = _pagoRepository.ObtenerPorReserva(id);
            return View(lista);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var pago = _pagoRepository.ObtenerPorId(id);

            if (pago == null)
                return NotFound();

            return View(pago);
        }


        //crear pago
        [HttpGet]
        public IActionResult Create(int id)
        {
            var pago = new Pago()
            { idReserva = id,
            fechaPago = DateTime.Now.Date };
            return View(pago);
        }

        [HttpPost]
        public IActionResult Create(Pago pago)
        {
            try
            {
                string idString = User.FindFirstValue(ClaimTypes.NameIdentifier);


                pago.idUsuarioCreador = int.Parse(idString); 
                _pagoRepository.GuardarPago(pago);

                return RedirectToAction("PorReserva", new { id = pago.idReserva });
            } catch (Exception ex)
            {
                Console.WriteLine("Error al hacer el pago: " + ex.ToString());
                return View(pago);
            }
        }


        
        // EDITAR (Solo Concepto)
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var pago = _pagoRepository.ObtenerPorId(id);
            // Si no existe o ya está anulado, no permitimos editarlo
            if (pago == null || !pago.estado) return NotFound();

            return View(pago);
        }

        [HttpPost]
        public IActionResult Edit(Pago pago)
        {
            _pagoRepository.ModificarConcepto(pago.idPago, pago.concepto);
            return RedirectToAction("PorReserva", new { id = pago.idReserva });
        }

        
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Anular(int id)
        {
            var pago = _pagoRepository.ObtenerPorId(id);
            if (pago == null || !pago.estado) return NotFound();
            return View(pago);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult AnularConfirmado(int IdPago, int IdReserva)
        {
            string idString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int idUsuarioAnulador = int.Parse(idString);

            _pagoRepository.AnularPago(IdPago, idUsuarioAnulador);

            return RedirectToAction("PorReserva", new { id = IdReserva });
        }

    }
}