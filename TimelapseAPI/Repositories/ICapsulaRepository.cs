using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{
    public interface ICapsulaRepository
    {
        Task<List<Capsula>> GetAllAsync();
        Task<Capsula?> GetByIdAsync(int id);
        Task<List<Capsula>> GetAllFilteredAsync(string? titulo, string? estado, string? orderBy, bool ascending);
        Task CreateAsync(Capsula capsula);
        Task<Capsula?> UpdateAsync(Capsula capsula);
        Task<bool> DeleteAsync(int id);
        Task<List<Capsula>> GetByUsuarioAsync(int idUsuario);

        

       
       
    }
}