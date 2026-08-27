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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inquilino inquilino)
        {
            if (!ModelState.IsValid)
            {
                return View(inquilino);
            }

            _inquilinoRepository.Guardar(inquilino);

            return RedirectToAction(nameof(Index));
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Inquilino inquilino)
        {
            if (!ModelState.IsValid)
            {
                return View(inquilino);
            }

            _inquilinoRepository.Modificar(inquilino);

            return RedirectToAction(nameof(Index));
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
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmado(int idInquilino)
        {
            Inquilino inquilino =
                _inquilinoRepository.ObtenerPorId(idInquilino);

            if (inquilino == null)
            {
                return NotFound();
            }

            _inquilinoRepository.Eliminar(idInquilino);

            return RedirectToAction(nameof(Index));
        }
    }
}