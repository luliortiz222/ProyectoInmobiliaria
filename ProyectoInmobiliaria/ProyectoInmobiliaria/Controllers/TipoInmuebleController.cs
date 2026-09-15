using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.models;
using ProyectoInmobiliaria.Repository;

namespace ProyectoInmobiliaria.Controllers
{
    [Authorize]
    public class TipoInmueblesController : Controller
    {
        private readonly TipoInmuebleRepository _tipoInmuebleRepository;

        public TipoInmueblesController(TipoInmuebleRepository tipoInmuebleRepository)
        {
            _tipoInmuebleRepository = tipoInmuebleRepository;
        }

        public IActionResult Index(string busqueda, int pagina = 1)
        {
            int cantidadPorPagina = 10;

            int totalTipos = _tipoInmuebleRepository.ContarTiposInmueble(busqueda);

            int totalPaginas = (int)Math.Ceiling(
                (double)totalTipos / cantidadPorPagina);

            if (totalPaginas > 0 && pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }

            if (pagina < 1)
            {
                pagina = 1;
            }

            var lista = _tipoInmuebleRepository.ObtenerPaginados(
                busqueda,
                pagina,
                cantidadPorPagina);

            ViewBag.Busqueda = busqueda;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public IActionResult DeleteConfirmed(int id)
        {
            _tipoInmuebleRepository.Eliminar(id);

            return RedirectToAction("Index");
        }
    }
}
