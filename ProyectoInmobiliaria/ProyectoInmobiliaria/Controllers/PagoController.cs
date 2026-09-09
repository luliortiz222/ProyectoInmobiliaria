using ProyectoInmobiliaria.models;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Repository;
using System;

namespace ProyectoInmobiliaria.Controllers
{
    public class PagoController : Controller
    {
        private readonly PagoRepository _pagoRepository;
        public PagoController(PagoRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }

        // GET: /Pagos
        [HttpGet]
        public IActionResult Index()
        {
            var pagos = _pagoRepository.ObtenerTodos();
            return View(pagos);
        }


        [HttpGet]
        public IActionResult PorReserva(int id)
        {
            ViewBag.idReserva = id;
            var lista = _pagoRepository.ObtenerPorReserva(id);
            return View(lista);
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
                pago.idUsuarioCreador = 1;
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
        public IActionResult Anular(int id)
        {
            var pago = _pagoRepository.ObtenerPorId(id);
            if (pago == null || !pago.estado) return NotFound();
            return View(pago);
        }

        [HttpPost]
        public IActionResult AnularConfirmado(int IdPago, int IdReserva)
        {
            // SIMULACIÓN: El usuario ID 1 es quien anula el pago
            int idUsuarioAnulador = 1;

            _pagoRepository.AnularPago(IdPago, idUsuarioAnulador);
            return RedirectToAction("PorReserva", new { id = IdReserva });
        }

    }
}