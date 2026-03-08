using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;

namespace TimelapseAPI.Repositories
{
    public interface IUsuarioCapsulaRepository
    {
        Task<List<UsuarioCapsula>> GetAllAsync();
        Task<UsuarioCapsula?> GetByIdAsync(int id);
        Task<List<UsuarioCapsula>> GetByCapsulaAsync(int idCapsula);
        Task<UsuarioCapsula> CreateAsync(UsuarioCapsula usuarioCapsula);
        Task<UsuarioCapsula?> UpdateAsync(UsuarioCapsula usuarioCapsula);
        Task<bool> DeleteAsync(int id);
    }
}