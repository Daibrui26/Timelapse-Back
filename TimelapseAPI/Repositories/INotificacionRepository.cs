using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{
    public interface INotificacionRepository
    {
        Task<List<Notificacion>> GetAllAsync();
        Task<Notificacion?> GetByIdAsync(int id);
        Task<Notificacion> CreateAsync(Notificacion notificacion);
        Task<Notificacion?> UpdateAsync(Notificacion notificacion);
        Task<bool> DeleteAsync(int id);
    }
}