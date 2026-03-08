using TimelapseAPI.Models;
using TimelapseAPI.Models.DTOs;
using TimelapseAPI.Repositories;

namespace TimelapseAPI.Services
{
    public class ContenidoService : IContenidoService
    {
        private readonly IContenidoRepository _repo;
        private readonly IUploadService _uploadService;

        public ContenidoService(IContenidoRepository repo, IUploadService uploadService)
        {
            _repo          = repo;
            _uploadService = uploadService;
        }

        public Task<List<Contenido>> GetAllAsync()               => _repo.GetAllAsync();
        public Task<Contenido?> GetByIdAsync(int id)             => _repo.GetByIdAsync(id);
        public Task<List<Contenido>> GetByCapsulaIdAsync(int id) => _repo.GetByCapsulaIdAsync(id);

        // ── Texto ──────────────────────────────────────────────────────────────
        public async Task<Contenido> CreateTextoAsync(Contenido contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido.ContenidoTexto))
                throw new ArgumentException("El texto no puede estar vacío.");

            if (contenido.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            contenido.Tipo        = "texto";
            contenido.FechaSubida = DateTime.UtcNow;

            return await _repo.CreateAsync(contenido);
        }

        // ── Archivo (imagen / vídeo / documento) ───────────────────────────────
        public async Task<Contenido> CreateArchivoAsync(ContenidoArchivoCreateDTO dto)
        {
            if (dto.IdCapsula <= 0)
                throw new ArgumentException("El IdCapsula debe ser válido.");

            var tiposValidos = new[] { "imagen", "video", "documento" };
            if (!tiposValidos.Contains(dto.Tipo.ToLower()))
                throw new ArgumentException("Tipo debe ser: imagen, video o documento.");

            // Subir a Cloudinary → obtenemos la URL
            var url = await _uploadService.UploadAsync(dto.Archivo);

            var contenido = new Contenido
            {
                Tipo        = dto.Tipo.ToLower(),
                UrlArchivo  = url,
                FechaSubida = DateTime.UtcNow,
                IdCapsula   = dto.IdCapsula
            };

            return await _repo.CreateAsync(contenido);
        }

        // ── Delete ─────────────────────────────────────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            var contenido = await _repo.GetByIdAsync(id);
            if (contenido == null) return false;

            // Si tiene archivo en Cloudinary, borrarlo también
            if (!string.IsNullOrEmpty(contenido.PublicId))
                await _uploadService.DeleteAsync(contenido.PublicId);

            return await _repo.DeleteAsync(id);
        }
    }
}