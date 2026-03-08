using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{

    public class AmistadService : IAmistadService
    {
        private readonly IAmistadRepository _amistadRepository;

        public AmistadService(IAmistadRepository amistadRepository)
        {
            _amistadRepository = amistadRepository;
        }

        public async Task<List<Amistad>> GetAllAsync()
        {
            return await _amistadRepository.GetAllAsync();
        }

        public async Task<Amistad?> GetByIdAsync(int id)
        {
            return await _amistadRepository.GetByIdAsync(id);
        }

        public async Task<Amistad> CreateAsync(Amistad amistad)
        {
            // Validación: no permitir amistad consigo mismo
            if (amistad.IdUsuario1 == amistad.IdUsuario2)
                throw new ArgumentException("Un usuario no puede ser amigo de sí mismo.");

            // Validación: estado por defecto si no se asigna
            if (string.IsNullOrEmpty(amistad.Estado))
                amistad.Estado = "pendiente";

            // Aquí podrías agregar más validaciones, como:
            // - verificar que los usuarios existan en la tabla Usuario
            // - verificar que no exista ya la amistad

            return await _amistadRepository.CreateAsync(amistad);
        }

        public async Task<Amistad?> UpdateAsync(Amistad amistad)
        {
            // Validación: no permitir amistad consigo mismo
            if (amistad.IdUsuario1 == amistad.IdUsuario2)
                throw new ArgumentException("Un usuario no puede ser amigo de sí mismo.");

            return await _amistadRepository.UpdateAsync(amistad);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _amistadRepository.DeleteAsync(id);
        }
    }
}