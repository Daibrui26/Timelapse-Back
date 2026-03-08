using Microsoft.AspNetCore.Mvc;
using TimelapseAPI.Models;
using TimelapseAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapsulaController : ControllerBase
    {
        private readonly ICapsulaService _capsulaService;

        public CapsulaController(ICapsulaService capsulaService)
        {
            _capsulaService = capsulaService;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<List<Capsula>>> GetAll()
        {
            var capsulas = await _capsulaService.GetAllAsync();
            return Ok(capsulas);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Capsula>> GetById(int id)
        {
            var capsula = await _capsulaService.GetByIdAsync(id);
            if (capsula == null)
                return NotFound();

            return Ok(capsula);
        }

        // GET: api/Capsula/usuario/5
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<Capsula>>> GetByUsuario(int idUsuario)
        {
            try
            {
                var capsulas = await _capsulaService.GetByUsuarioAsync(idUsuario);
                return Ok(capsulas);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST - CREATE
        [HttpPost]
        public async Task<ActionResult<Capsula>> Create(Capsula capsula)
        {
            await _capsulaService.AddAsync(capsula);
            return CreatedAtAction(nameof(GetById), new { id = capsula.IdCapsula }, capsula);
        }

        // PUT - UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Capsula updatedCapsula)
        {
            var existingCapsula = await _capsulaService.GetByIdAsync(id);
            if (existingCapsula == null)
                return NotFound();

            // Actualizar los campos
            existingCapsula.Titulo = updatedCapsula.Titulo;
            existingCapsula.Descripcion = updatedCapsula.Descripcion;
            existingCapsula.FechaCreacion = updatedCapsula.FechaCreacion;
            existingCapsula.FechaApertura = updatedCapsula.FechaApertura;
            existingCapsula.Estado = updatedCapsula.Estado;
            existingCapsula.Visibilidad = updatedCapsula.Visibilidad;

            await _capsulaService.UpdateAsync(existingCapsula);
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var capsula = await _capsulaService.GetByIdAsync(id);
            if (capsula == null)
                return NotFound();

            await _capsulaService.DeleteAsync(id);
            return NoContent();
        }

        // GET FILTERED - SEARCH
        [HttpGet("search")]
        public async Task<ActionResult<List<Capsula>>> Search(
            [FromQuery] string? titulo,
            [FromQuery] string? estado,
            [FromQuery] string? orderBy,
            [FromQuery] bool ascending = true)
        {
            try
            {
                var capsulas = await _capsulaService.GetAllFilteredAsync(titulo, estado, orderBy, ascending);
                return Ok(capsulas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}