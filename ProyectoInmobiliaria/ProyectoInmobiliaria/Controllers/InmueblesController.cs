using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Repository;
using System;

namespace ProyectoInmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly InmuebleRepository _inmuebleRepository;
        private readonly PropietarioRepository _propietarioRepo;
        private readonly TipoInmuebleRepository _tipoRepo;
        public InmuebleController(InmuebleRepository inmuebleRepository, PropietarioRepository propietarioRepo, TipoInmuebleRepository tipoRepo)
        {
            _inmuebleRepository = inmuebleRepository;
            _propietarioRepo = propietarioRepo;
            _tipoRepo = tipoRepo;
        }


        //Get: /Inmuebles
        public IActionResult Index() { 
            var inmuebles  = _inmuebleRepository.ObtenerTodos();
            return View(inmuebles);
        }

        //GEt: /Inmuebles/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Propietarios = _propietarioRepo.ObtenerTodos();
            ViewBag.TiposInmueble = _tipoRepo.ObtenerTodos();
            return View();
        }

        //POST: /Inmuebles/Create
        [HttpPost]
        public IActionResult Create(Inmueble inmueble)
            _inmuebleRepository.Guardar(inmueble);
            return RedirectToAction("Index");

        }

        //GET: /Inmueble/Edit/5
        [HttpGet]
        public IActionResult Edit(int  id)
        {
            Inmueble inmueble = _inmuebleRepository.obtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            ViewBag.Propietario = _inmuebleRepository.ObtenerTodos();
            ViewBag.TipoInmuebles = _tipoRepo.ObtenerTodos();
            return View(inmueble);
        }

        //POST: /Inmuebles/Edit
        [HttpPost]
        public IActionResult Edit(Inmuebles inmuebles) {
            _inmuebleRepository.Actualizar(inmuebles);
            return RedirectToAction("Index" );
        }

        // GET: /Inmuebles/Borrar/5
        [HttpGet]
        public IActionResult Borrar(int id)
        {
            var inmueble = _inmuebleRepo.ObtenerPorId(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        // POST: /Inmuebles/BorrarConfirmado
        [HttpPost]
        public IActionResult BorrarConfirmado(int IdInmueble)
        {
            _inmuebleRepo.Eliminar(IdInmueble);
            return RedirectToAction("Index");
        }

    }
}  
