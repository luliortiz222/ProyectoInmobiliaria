using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoInmobiliaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta: api/inquilinos
    public class InquilinosController : ControllerBase
    {
        private readonly InquilinoRepository _inquilinoRepository;

        public InquilinosController(IConfiguration config)
        {
            string cadenaConexion = config.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;Database=inmobiliaria;Uid=root;Pwd=admin;";
            _inquilinoRepository = new InquilinoRepository(cadenaConexion);
        }

        // GET api/inquilinos
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            List<Inquilino> lista = _inquilinoRepository.ObtenerTodos();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            Inquilino inquilino = _inquilinoRepository.ObtenerPorId(id);
            if (inquilino == null)
            {
                return NotFound("Inquilino no encontrado.");
            }
            return Ok(inquilino);
        }

        // POST api/inquilinos
        [HttpPost]
        public IActionResult Crear([FromBody] Inquilino inquilino)
        {
            if (inquilino == null)
            {
                return BadRequest("Datos inválidos.");
            }

            _inquilinoRepository.Guardar(inquilino);
            return Ok("Inquilino creado exitosamente.");
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Inquilino inquilino)
        {
            if (inquilino == null)
            {
                return BadRequest("Datos inválidos.");
            }

            inquilino.IdInquilino = id;
            _inquilinoRepository.Modificar(inquilino);
            return Ok("Inquilino actualizado exitosamente.");
        }

        // DELETE api/inquilinos/5
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            Inquilino inquilinoExistente = _inquilinoRepository.ObtenerPorId(id);
            if (inquilinoExistente == null)
            {
                return NotFound("El inquilino a eliminar no existe.");
            }

            _inquilinoRepository.Eliminar(id);
            return Ok("Inquilino eliminado correctamente.");
        }
    }
}
