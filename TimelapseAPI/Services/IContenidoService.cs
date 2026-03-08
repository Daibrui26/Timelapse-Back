using TimelapseAPI.Models;
using TimelapseAPI.Models.DTOs;

namespace TimelapseAPI.Services
{
    public interface IContenidoService
    {
        Task<List<Contenido>> GetAllAsync();
        Task<Contenido?> GetByIdAsync(int id);
        Task<List<Contenido>> GetByCapsulaIdAsync(int idCapsula);

        ///Crea un contenido de tipo texto.
        Task<Contenido> CreateTextoAsync(Contenido contenido);

        ///Sube un archivo a Cloudinary y guarda la URL en BD.
        Task<Contenido> CreateArchivoAsync(ContenidoArchivoCreateDTO dto);

        ///Elimina el registro de BD y el archivo de Cloudinary si existe.
        Task<bool> DeleteAsync(int id);
    }
}