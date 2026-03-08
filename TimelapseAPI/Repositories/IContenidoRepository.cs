using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{
    public interface IContenidoRepository
    {
        Task<List<Contenido>> GetAllAsync();
        Task<Contenido?> GetByIdAsync(int id);
        Task<List<Contenido>> GetByCapsulaIdAsync(int idCapsula);
        Task<Contenido> CreateAsync(Contenido contenido);
        Task<bool> DeleteAsync(int id);
    }
}