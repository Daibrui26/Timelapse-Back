using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<List<Usuario>> GetAllFilteredAsync(string? nombre, string? email, string? orderBy, bool ascending);
        Task CreateAsync(Usuario usuario);
        Task<Usuario?> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }
}