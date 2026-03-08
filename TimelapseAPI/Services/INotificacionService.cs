using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public interface INotificacionService
    {
        Task<List<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(int id);
        Task<Notificacion> CreateAsync(Notificacion notificacion);
        Task<Notificacion?> UpdateAsync(Notificacion notificacion);
        Task<bool> DeleteAsync(int id);
    }
}