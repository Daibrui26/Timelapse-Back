using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _notificacionRepository;

        public NotificacionService(INotificacionRepository notificacionRepository)
        {
            _notificacionRepository = notificacionRepository;
        }

        public async Task<List<Notificacion>> GetAllAsync()
        {
            return await _notificacionRepository.GetAllAsync();
        }

        public async Task<Notificacion?> GetByIdAsync(int id)
        {
            return await _notificacionRepository.GetByIdAsync(id);
        }

        public async Task<Notificacion> CreateAsync(Notificacion notificacion)
        {
            // Validaciones antes de crear la notificación
            if (string.IsNullOrWhiteSpace(notificacion.Tipo))
                throw new ArgumentException("El tipo de notificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(notificacion.Mensaje))
                throw new ArgumentException("El mensaje de la notificación no puede estar vacío.");

            if (notificacion.IdUsuario <= 0)
                throw new ArgumentException("El IdUsuario debe ser válido.");

            // Si no se asigna fecha, se pone la actual
            if (notificacion.FechaCreacion == default)
                notificacion.FechaCreacion = DateTime.UtcNow;

            return await _notificacionRepository.CreateAsync(notificacion);
        }

        public async Task<Notificacion?> UpdateAsync(Notificacion notificacion)
        {
            // Validaciones similares a CreateAsync
            if (string.IsNullOrWhiteSpace(notificacion.Tipo))
                throw new ArgumentException("El tipo de notificación no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(notificacion.Mensaje))
                throw new ArgumentException("El mensaje de la notificación no puede estar vacío.");

            if (notificacion.IdUsuario <= 0)
                throw new ArgumentException("El IdUsuario debe ser válido.");

            return await _notificacionRepository.UpdateAsync(notificacion);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _notificacionRepository.DeleteAsync(id);
        }
    }
}