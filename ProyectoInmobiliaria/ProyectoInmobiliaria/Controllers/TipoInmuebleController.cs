using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;

namespace ProyectoInmobiliaria.Controllers
{
    public class TipoInmueblesController : Controller
    {
        private readonly TipoInmuebleRepository _tipoInmuebleRepository;

        public TipoInmueblesController(TipoInmuebleRepository tipoInmuebleRepository)
        {
            _tipoInmuebleRepository = tipoInmuebleRepository;
        }

        public IActionResult Index()
        {
            var lista = _tipoInmuebleRepository.ObtenerTodos();

            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TipoInmueble tipo)
        {
            _tipoInmuebleRepository.Guardar(tipo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tipo = _tipoInmuebleRepository.ObtenerPorId(id);

            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        [HttpPost]
        public IActionResult Edit(TipoInmueble tipo)
        {
            _tipoInmuebleRepository.Editar(tipo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var tipo = _tipoInmuebleRepository.ObtenerPorId(id);

            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var tipo = _tipoInmuebleRepository.ObtenerPorId(id);

            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _tipoInmuebleRepository.Eliminar(id);

            return RedirectToAction("Index");
        }
    }
}
