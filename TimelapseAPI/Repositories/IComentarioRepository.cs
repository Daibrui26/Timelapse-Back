using TimelapseAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimelapseAPI.Repositories
{

public interface IComentarioRepository
    {
        Task<List<Comentario>> GetAllAsync();
        Task<Comentario?> GetByIdAsync(int id);
        Task<Comentario> CreateAsync(Comentario comentario);
        Task<Comentario?> UpdateAsync(Comentario comentario);
        Task<bool> DeleteAsync(int id);
        Task<List<Comentario>> GetByCapsulaAsync(int idCapsula);
    }
    }