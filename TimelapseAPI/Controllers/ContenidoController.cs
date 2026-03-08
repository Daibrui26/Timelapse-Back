using Microsoft.AspNetCore.Mvc;
using TimelapseAPI.Models;
using TimelapseAPI.Models.DTOs;
using TimelapseAPI.Services;

namespace TimelapseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContenidoController : ControllerBase
    {
        private readonly IContenidoService _contenidoService;

        public ContenidoController(IContenidoService contenidoService)
        {
            _contenidoService = contenidoService;
        }

        // GET api/Contenido
        [HttpGet]
        public async Task<ActionResult<List<Contenido>>> GetAll()
        {
            var lista = await _contenidoService.GetAllAsync();
            return Ok(lista);
        }

        // GET api/Contenido/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Contenido>> GetById(int id)
        {
            var contenido = await _contenidoService.GetByIdAsync(id);
            if (contenido == null) return NotFound();
            return Ok(contenido);
        }

        // GET api/Contenido/capsula/{idCapsula}
        [HttpGet("capsula/{idCapsula:int}")]
        public async Task<ActionResult<List<Contenido>>> GetByCapsula(int idCapsula)
        {
            var lista = await _contenidoService.GetByCapsulaIdAsync(idCapsula);
            return Ok(lista);
        }

        // POST api/Contenido/texto
        // Body: { "idCapsula": 1, "contenidoTexto": "Querido yo del futuro..." }
        [HttpPost("texto")]
        public async Task<ActionResult<Contenido>> CreateTexto([FromBody] Contenido contenido)
        {
            try
            {
                var nuevo = await _contenidoService.CreateTextoAsync(contenido);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.IdContenido }, nuevo);
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

        // POST api/Contenido/archivo
        // multipart/form-data: Archivo (IFormFile), IdCapsula (int), Tipo ("imagen"/"video"/"documento")
        [HttpPost("archivo")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Contenido>> CreateArchivo([FromForm] ContenidoArchivoCreateDTO dto)
        {
            try
            {
                var nuevo = await _contenidoService.CreateArchivoAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = nuevo.IdContenido }, nuevo);
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

        // DELETE api/Contenido/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contenidoService.DeleteAsync(id);
            if (!result) return NotFound(new { mensaje = "No se encontró el contenido con ese ID." });
            return NoContent();
        }
    }
}