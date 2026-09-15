using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoInmobiliaria.models;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;

namespace ProyectoInmobiliaria.Controllers
{

    [Authorize]
    public class PropietariosController : Controller
    {
        private readonly PropietarioRepository _propietarioRepository;
        public PropietariosController(PropietarioRepository propietarioRepository)
        {
            _propietarioRepository = propietarioRepository;
        }





        // GET: /Propietarios (Muestra la lista)
        public IActionResult Index(string busqueda, int pagina = 1)
        {
            int cantidadPorPagina = 10;

            int totalPropietarios = _propietarioRepository.ContarPropietarios(busqueda);

            int totalPaginas = (int)Math.Ceiling(
                (double)totalPropietarios / cantidadPorPagina
            );

            if (totalPaginas > 0 && pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }

            if (pagina < 1)
            {
                pagina = 1;
            }

            var lista = _propietarioRepository.ObtenerPaginados(
                busqueda,
                pagina,
                cantidadPorPagina
            );

            ViewBag.Busqueda = busqueda;
            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(lista);
        }

        // GET: /Propietarios/Crear (Muestra el formulario vacío)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Propietarios/Crear (Recibe los datos del formulario)
        [HttpPost]
        public IActionResult Create(Propietario propietario)
        {
            _propietarioRepository.guardar(propietario);
            return RedirectToAction("Index"); // Vuelve a la lista tras guardar
        }


        

      

        // GET: /Propietarios/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var propietario = _propietarioRepository.obtenerPorId(id);
            if (propietario == null)
            {
                return NotFound(); 
            }
            return View(propietario); 
        }

        // POST: /Propietarios/Edit
        [HttpPost]
        public IActionResult Edit(Propietario propietario)
        {
            _propietarioRepository.actualizar(propietario);
            return RedirectToAction("Index");
        }




        // GET: /Propietarios/Borrar/5
        // Este método busca al propietario y muestra la pantalla de advertencia
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int id)
        {
            var propietario = _propietarioRepository.obtenerPorId(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // POST: /Propietarios/BorrarConfirmado
        // Este método es el que realmente elimina el registro de MySQL
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public IActionResult BorrarConfirmado(int IdPropietario)
        {
            _propietarioRepository.eliminar(IdPropietario);
            return RedirectToAction("Index"); 
        }
    }
}
