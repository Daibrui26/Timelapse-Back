using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public interface IComentarioService
    {
        Task<List<Comentario>> GetAllAsync();
        Task<Comentario?> GetByIdAsync(int id);
        Task<Comentario> CreateAsync(Comentario comentario);
        Task<Comentario?> UpdateAsync(Comentario comentario);
        Task<bool> DeleteAsync(int id);
        Task<List<Comentario>> GetByCapsulaAsync(int idCapsula);
    }
}