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
    public class AmistadController : ControllerBase
    {
        private readonly IAmistadService _amistadService;

        public AmistadController(IAmistadService amistadService)
        {
            _amistadService = amistadService;
        }

        // GET: api/Amistad
        [HttpGet]
        public async Task<ActionResult<List<Amistad>>> GetAll()
        {
            var amistades = await _amistadService.GetAllAsync();
            return Ok(amistades);
        }

        // GET: api/Amistad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amistad>> GetById(int id)
        {
            var amistad = await _amistadService.GetByIdAsync(id);
            if (amistad == null)
                return NotFound();
            return Ok(amistad);
        }

        // POST: api/Amistad
        [HttpPost]
        public async Task<ActionResult<Amistad>> Create([FromBody] Amistad amistad)
        {
            try
            {
                var nuevaAmistad = await _amistadService.CreateAsync(amistad);
                return CreatedAtAction(nameof(GetById), new { id = nuevaAmistad.IdAmistad }, nuevaAmistad);
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

        // PUT: api/Amistad/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Amistad>> Update(int id, [FromBody] Amistad amistad)
        {
            if (id != amistad.IdAmistad)
                return BadRequest(new { mensaje = "El ID no coincide con el cuerpo de la solicitud." });

            try
            {
                var updated = await _amistadService.UpdateAsync(amistad);
                if (updated == null)
                    return NotFound();
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

        // DELETE: api/Amistad/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _amistadService.DeleteAsync(id);
            if (!result)
                return NotFound(new { mensaje = "No se encontró la amistad con ese ID." });
            return NoContent();
        }
    }
}