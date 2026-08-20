using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;


namespace ProyectoInmobiliaria.Controllers
{
    [ApiController]
    [Route("api/propietario")]
    public class PropietariosController : ControllerBase
    {
        private readonly PropietarioRepository _propietarioRepository;
        public PropietariosController(PropietarioRepository propietarioRepository)
        {
            _propietarioRepository = propietarioRepository;
        }

        
        /*
         * -http://localhost:5000/ 
         */


        // GET api/propietario
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<Propietario> lista = _propietarioRepository.obtenerTodos();
            return Ok(lista);   
        }

        // GET api/propietario/dni/123213
        [HttpGet("dni/{dni}")]
        public IActionResult obtenerPorDni(string dni)
        {
            Propietario propietario = _propietarioRepository.obtenerPorDni(dni);
            if (propietario == null)
            {
                return NotFound("Propietario no encontrado");
            }
            return Ok(propietario);
        }

        //POST: api/propietario
        [HttpPost]
        public IActionResult Crear([FromBody] Propietario propietario) {
            _propietarioRepository.guardar(propietario);
            return Ok("Propietario creado exitosamente");
        }


        //PUT: api/propietario/5
        [HttpPut("{id}")]
        public IActionResult Actualizar([FromBody] Propietario propietario)
        {
            _propietarioRepository.actualizar(propietario);
            return Ok("Propietario actualizado exitosamente");
        }

        // DELETE: api/propietario/5
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            _propietarioRepository.eliminar(id);
            return Ok("Propietario eliminado correctamente.");
        }
    }
}
