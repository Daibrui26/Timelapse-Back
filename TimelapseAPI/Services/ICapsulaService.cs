using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Services
{
    public interface ICapsulaService
    {
        Task<List<Capsula>> GetAllAsync();
        Task<Capsula?> GetByIdAsync(int id);
        Task AddAsync(Capsula capsula);
        Task UpdateAsync(Capsula capsula);
        Task DeleteAsync(int id);

        // Búsqueda filtrada y ordenación
        Task<List<Capsula>> GetAllFilteredAsync(string? titulo, string? estado, string? orderBy, bool ascending);
        Task<List<Capsula>> GetByUsuarioAsync(int idUsuario);    
    
    }
}