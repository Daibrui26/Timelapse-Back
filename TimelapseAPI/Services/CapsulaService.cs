using TimelapseAPI.Models;
using TimelapseAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Services
{
    public class CapsulaService : ICapsulaService
    {
        private readonly ICapsulaRepository _capsulaRepository;

        public CapsulaService(ICapsulaRepository capsulaRepository)
        {
            _capsulaRepository = capsulaRepository;
        }

        // CRUD básico

        public async Task<List<Capsula>> GetAllAsync()
        {
            return await _capsulaRepository.GetAllAsync();
        }

        public async Task<Capsula?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor que cero.");

            return await _capsulaRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Capsula capsula)
        {
            if (string.IsNullOrWhiteSpace(capsula.Titulo))
                throw new ArgumentException("El título de la cápsula no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(capsula.Estado))
                throw new ArgumentException("El estado de la cápsula no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(capsula.Visibilidad))
                throw new ArgumentException("La visibilidad de la cápsula no puede estar vacía.");

            await _capsulaRepository.CreateAsync(capsula);
        }

        public async Task UpdateAsync(Capsula capsula)
        {
            if (capsula.IdCapsula <= 0)
                throw new ArgumentException("El ID no es válido para actualización.");

            if (string.IsNullOrWhiteSpace(capsula.Titulo))
                throw new ArgumentException("El título de la cápsula no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(capsula.Estado))
                throw new ArgumentException("El estado de la cápsula no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(capsula.Visibilidad))
                throw new ArgumentException("La visibilidad de la cápsula no puede estar vacía.");

            var updated = await _capsulaRepository.UpdateAsync(capsula);
            if (updated == null)
                throw new KeyNotFoundException("Cápsula no encontrada para actualizar.");
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID no es válido para eliminación.");

            var deleted = await _capsulaRepository.DeleteAsync(id);
            if (!deleted)
                throw new KeyNotFoundException("Cápsula no encontrada para eliminar.");
        }

        // Búsqueda filtrada delegada al Repository
        public async Task<List<Capsula>> GetAllFilteredAsync(string? titulo, string? estado, string? orderBy, bool ascending)
        {
            return await _capsulaRepository.GetAllFilteredAsync(titulo, estado, orderBy, ascending);
        }
        
        public async Task<List<Capsula>> GetByUsuarioAsync(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID de usuario no es válido.");

            return await _capsulaRepository.GetByUsuarioAsync(idUsuario);
        }
    }
}