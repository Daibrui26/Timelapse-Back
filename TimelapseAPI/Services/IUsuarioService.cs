using TimelapseAPI.Models;
using TimelapseAPI.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<List<Usuario>> GetAllFilteredAsync(string? nombre, string? email, string? orderBy, bool ascending);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
    }
}