using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimelapseAPI.Models;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public interface IAmistadService
    {
        Task<List<Amistad>> GetAllAsync();
        Task<Amistad?> GetByIdAsync(int id);
        Task<Amistad> CreateAsync(Amistad amistad);
        Task<Amistad?> UpdateAsync(Amistad amistad);
        Task<bool> DeleteAsync(int id);
    }
}