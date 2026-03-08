using TimelapseAPI.Models;
using TimelapseAPI.Models.DTOs;
using TimelapseAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<List<Usuario>> GetAllFilteredAsync(string? nombre, string? email, string? orderBy, bool ascending)
        {
            return await _usuarioRepository.GetAllFilteredAsync(nombre, email, orderBy, ascending);
        }

        public async Task AddAsync(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("El nombre del usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("El email del usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Contraseña))
                throw new ArgumentException("La contraseña del usuario no puede estar vacía.");

            await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            if (usuario.IdUsuario <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("El nombre del usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new ArgumentException("El email del usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(usuario.Contraseña))
                throw new ArgumentException("La contraseña del usuario no puede estar vacía.");

            var updated = await _usuarioRepository.UpdateAsync(usuario);
            if (updated == null)
                throw new KeyNotFoundException("Usuario no encontrado para actualizar.");
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            var deleted = await _usuarioRepository.DeleteAsync(id);
            if (!deleted)
                throw new KeyNotFoundException("Usuario no encontrado para eliminar.");
        }

        // LOGIN: busca por email y compara contraseña en texto plano
        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Contraseña))
                throw new ArgumentException("Email y contraseña son obligatorios.");

            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

            if (usuario == null || usuario.Contraseña != request.Contraseña)
                return null; // Credenciales incorrectas

            return new LoginResponseDTO
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }
    }
}