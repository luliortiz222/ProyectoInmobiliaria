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
    
    public class PropietariosController : Controller
    {
        private readonly PropietarioRepository _propietarioRepository;
        public PropietariosController(PropietarioRepository propietarioRepository)
        {
            _propietarioRepository = propietarioRepository;
        }


       


        // GET: /Propietarios (Muestra la lista)
        public IActionResult Index()
        {
            var lista = _propietarioRepository.obtenerTodos();
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
        public IActionResult BorrarConfirmado(int IdPropietario)
        {
            _propietarioRepository.eliminar(IdPropietario);
            return RedirectToAction("Index"); 
        }
    }
}
