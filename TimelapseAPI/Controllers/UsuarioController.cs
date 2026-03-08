using Microsoft.AspNetCore.Mvc;
using TimelapseAPI.Models;
using TimelapseAPI.Models.DTOs;
using TimelapseAPI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TimelapseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // POST: api/Usuario/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                var result = await _usuarioService.LoginAsync(request);
                if (result == null)
                    return Unauthorized(new { mensaje = "Email o contraseña incorrectos." });

                return Ok(result);
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

        // GET: api/Usuario
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound($"Usuario con ID {id} no encontrado.");

                return Ok(usuario);
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

        // GET: api/Usuario/search?nombre=Juan&email=correo
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? nombre,
            [FromQuery] string? email)
        {
            try
            {
                var usuarios = await _usuarioService.GetAllAsync();

                if (!string.IsNullOrWhiteSpace(nombre))
                    usuarios = usuarios.Where(u => u.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!string.IsNullOrWhiteSpace(email))
                    usuarios = usuarios.Where(u => u.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            try
            {
                await _usuarioService.AddAsync(usuario);
                return CreatedAtAction(nameof(GetById), new { id = usuario.IdUsuario }, usuario);
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

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (id != usuario.IdUsuario)
                    return BadRequest("El ID de la ruta no coincide con el ID del usuario.");

                await _usuarioService.UpdateAsync(usuario);
                return Ok(usuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usuarioService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}