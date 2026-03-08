using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Services;

namespace TimelapseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioCapsulaController : ControllerBase
    {
        private readonly IUsuarioCapsulaService _usuarioCapsulaService;

        public UsuarioCapsulaController(IUsuarioCapsulaService usuarioCapsulaService)
        {
            _usuarioCapsulaService = usuarioCapsulaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioCapsula>>> GetAll()
        {
            var lista = await _usuarioCapsulaService.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioCapsula>> GetById(int id)
        {
            var item = await _usuarioCapsulaService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/UsuarioCapsula/capsula/5
        [HttpGet("capsula/{idCapsula}")]
        public async Task<ActionResult<List<UsuarioCapsula>>> GetByCapsula(int idCapsula)
        {
            var lista = await _usuarioCapsulaService.GetByCapsulaAsync(idCapsula);
            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioCapsula>> Create([FromBody] UsuarioCapsula usuarioCapsula)
        {
            try
            {
                var nuevo = await _usuarioCapsulaService.CreateAsync(usuarioCapsula);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.IdUsuarioCapsula }, nuevo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioCapsula>> Update(int id, [FromBody] UsuarioCapsula usuarioCapsula)
        {
            if (id != usuarioCapsula.IdUsuarioCapsula)
                return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

            try
            {
                var updated = await _usuarioCapsulaService.UpdateAsync(usuarioCapsula);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _usuarioCapsulaService.DeleteAsync(id);
            if (!result) return NotFound(new { mensaje = "No se encontró el registro con ese ID." });
            return NoContent();
        }
    }
}