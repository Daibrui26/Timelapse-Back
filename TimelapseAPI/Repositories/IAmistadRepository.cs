using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{
    public interface IAmistadRepository
    {
        Task<List<Amistad>> GetAllAsync();
        Task<Amistad?> GetByIdAsync(int id);
        Task<Amistad> CreateAsync(Amistad amistad);
        Task<Amistad?> UpdateAsync(Amistad amistad);
        Task<bool> DeleteAsync(int id);

        // Búsqueda filtrada con ordenación
        
    }
}