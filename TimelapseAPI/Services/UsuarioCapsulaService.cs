using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public class UsuarioCapsulaService : IUsuarioCapsulaService
    {
        private readonly IUsuarioCapsulaRepository _usuarioCapsulaRepository;

        public UsuarioCapsulaService(IUsuarioCapsulaRepository usuarioCapsulaRepository)
        {
            _usuarioCapsulaRepository = usuarioCapsulaRepository;
        }

        public async Task<List<UsuarioCapsula>> GetAllAsync()
        {
            return await _usuarioCapsulaRepository.GetAllAsync();
        }

        public async Task<UsuarioCapsula?> GetByIdAsync(int id)
        {
            return await _usuarioCapsulaRepository.GetByIdAsync(id);
        }

        public async Task<List<UsuarioCapsula>> GetByCapsulaAsync(int idCapsula)
        {
            return await _usuarioCapsulaRepository.GetByCapsulaAsync(idCapsula);
        }

        public async Task<UsuarioCapsula> CreateAsync(UsuarioCapsula usuarioCapsula)
        {
            if (usuarioCapsula.IdUsuario <= 0)
                throw new ArgumentException("El IdUsuario debe ser válido.");

            if (usuarioCapsula.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            if (string.IsNullOrWhiteSpace(usuarioCapsula.Rol))
                usuarioCapsula.Rol = "miembro";

            return await _usuarioCapsulaRepository.CreateAsync(usuarioCapsula);
        }

        public async Task<UsuarioCapsula?> UpdateAsync(UsuarioCapsula usuarioCapsula)
        {
            if (usuarioCapsula.IdUsuario <= 0)
                throw new ArgumentException("El IdUsuario debe ser válido.");

            if (usuarioCapsula.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            if (string.IsNullOrWhiteSpace(usuarioCapsula.Rol))
                usuarioCapsula.Rol = "miembro";

            return await _usuarioCapsulaRepository.UpdateAsync(usuarioCapsula);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _usuarioCapsulaRepository.DeleteAsync(id);
        }
    }
}