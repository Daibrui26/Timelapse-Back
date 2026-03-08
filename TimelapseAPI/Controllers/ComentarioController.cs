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
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;

        public ComentarioController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comentario>>> GetAll()
        {
            var comentarios = await _comentarioService.GetAllAsync();
            return Ok(comentarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetById(int id)
        {
            var comentario = await _comentarioService.GetByIdAsync(id);
            if (comentario == null) return NotFound();
            return Ok(comentario);
        }

        [HttpPost]
        public async Task<ActionResult<Comentario>> Create([FromBody] Comentario comentario)
        {
            try
            {
                var nuevo = await _comentarioService.CreateAsync(comentario);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.IdComentario }, nuevo);
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
        public async Task<ActionResult<Comentario>> Update(int id, [FromBody] Comentario comentario)
        {
            if (id != comentario.IdComentario)
                return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

            try
            {
                var updated = await _comentarioService.UpdateAsync(comentario);
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
            var result = await _comentarioService.DeleteAsync(id);
            if (!result) return NotFound(new { mensaje = "No se encontró el comentario con ese ID." });
            return NoContent();
        }

        [HttpGet("capsula/{idCapsula}")]
        public async Task<ActionResult<List<Comentario>>> GetByCapsula(int idCapsula)
        {
            var comentarios = await _comentarioService.GetByCapsulaAsync(idCapsula);
            return Ok(comentarios);
        }
    }
}