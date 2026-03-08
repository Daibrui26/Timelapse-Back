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
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notificacion>>> GetAll()
        {
            var lista = await _notificacionService.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notificacion>> GetById(int id)
        {
            var notif = await _notificacionService.GetByIdAsync(id);
            if (notif == null) return NotFound();
            return Ok(notif);
        }

        [HttpPost]
        public async Task<ActionResult<Notificacion>> Create([FromBody] Notificacion notificacion)
        {
            try
            {
                var nuevo = await _notificacionService.CreateAsync(notificacion);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.IdNotificacion }, nuevo);
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
        public async Task<ActionResult<Notificacion>> Update(int id, [FromBody] Notificacion notificacion)
        {
            if (id != notificacion.IdNotificacion)
                return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

            try
            {
                var updated = await _notificacionService.UpdateAsync(notificacion);
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
            var result = await _notificacionService.DeleteAsync(id);
            if (!result) return NotFound(new { mensaje = "No se encontró la notificación con ese ID." });
            return NoContent();
        }
    }
}