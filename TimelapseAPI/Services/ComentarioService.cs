using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{

    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _comentarioRepository;

        public ComentarioService(IComentarioRepository comentarioRepository)
        {
            _comentarioRepository = comentarioRepository;
        }

        public async Task<List<Comentario>> GetAllAsync()
        {
            return await _comentarioRepository.GetAllAsync();
        }

        public async Task<Comentario?> GetByIdAsync(int id)
        {
            return await _comentarioRepository.GetByIdAsync(id);
        }

        public async Task<Comentario> CreateAsync(Comentario comentario)
        {
            // Validaciones básicas antes de crear un comentario

            if (string.IsNullOrWhiteSpace(comentario.Texto))
                throw new ArgumentException("El texto del comentario no puede estar vacío.");

            if (comentario.IdUsuario <= 0)
                throw new ArgumentException("El IdUsuario debe ser válido.");

            if (comentario.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            // Si no se asigna fecha, se pone la actual
            if (comentario.FechaComentario == default)
                comentario.FechaComentario = DateTime.UtcNow;

            return await _comentarioRepository.CreateAsync(comentario);
        }

        public async Task<Comentario?> UpdateAsync(Comentario comentario)
        {
            // Validaciones similares a CreateAsync
            if (string.IsNullOrWhiteSpace(comentario.Texto))
                throw new ArgumentException("El texto del comentario no puede estar vacío.");

            if (comentario.IdUsuario <= 0)
                throw new ArgumentException("El IdUsuario debe ser válido.");

            if (comentario.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            return await _comentarioRepository.UpdateAsync(comentario);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _comentarioRepository.DeleteAsync(id);
        }

        public async Task<List<Comentario>> GetByCapsulaAsync(int idCapsula)
        {
            return await _comentarioRepository.GetByCapsulaAsync(idCapsula);
        }
    }
}