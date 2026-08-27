using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using System.Collections.Generic;

namespace ProyectoInmobiliaria.Controllers
{
    public class InquilinosController : Controller
    {
        private readonly InquilinoRepository _inquilinoRepository;

        public InquilinosController(InquilinoRepository inquilinoRepository)
        {
            _inquilinoRepository = inquilinoRepository;
        }

        // GET: /Inquilinos
        public IActionResult Index()
        {
            List<Inquilino> lista = _inquilinoRepository.ObtenerTodos();

            return View(lista);
        }

        // GET: /Inquilinos/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Inquilinos/Create
        
        [HttpPost]
        public IActionResult Create(Inquilino inquilino)
        {
            _inquilinoRepository.Guardar(inquilino);
            return RedirectToAction("Index"); // Para que vuelva a la lista tras guardar
        }

        // GET: /Inquilinos/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Inquilino inquilino = _inquilinoRepository.ObtenerPorId(id);

            if (inquilino == null)
            {
                return NotFound();
            }

            return View(inquilino);
        }

        // POST: /Inquilinos/Edit
        [HttpPost]
        public IActionResult Edit(Inquilino inquilino)
        {
            

            _inquilinoRepository.Modificar(inquilino);

            return RedirectToAction("Index");
        }

        // GET: /Inquilinos/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Inquilino inquilino = _inquilinoRepository.ObtenerPorId(id);

            if (inquilino == null)
            {
                return NotFound();
            }

            return View(inquilino);
        }

        // POST: /Inquilinos/Delete
        [HttpPost]
        public IActionResult DeleteConfirmado(int idInquilino)
        {
            Inquilino inquilino =
                _inquilinoRepository.ObtenerPorId(idInquilino);

            if (inquilino == null)
            {
                return NotFound();
            }

            _inquilinoRepository.Eliminar(idInquilino);

            return RedirectToAction("Index");
        }
    }
}