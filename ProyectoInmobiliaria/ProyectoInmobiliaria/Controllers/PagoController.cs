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


        [HttpGet]
        public IActionResult PorReserva(int id)
        {
            ViewBag.idReserva = id;
            var lista = _pagoRepository.ObtenerPorReserva(id);
            return View(lista);
        }


        //crear pago
        [HttpGet]
        public IActionResult Crear(int id)
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

    }
}