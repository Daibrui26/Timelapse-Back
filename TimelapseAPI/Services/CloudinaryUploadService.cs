using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TimelapseAPI.Configurations;
using Microsoft.Extensions.Options;

namespace TimelapseAPI.Services
{
    public class CloudinaryUploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryUploadService(IOptions<CloudinarySettings> config)
        {
            var c = config.Value;
            var account = new Account(c.CloudName, c.ApiKey, c.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadAsync(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                throw new ArgumentException("El archivo está vacío.");

            await using var stream = archivo.OpenReadStream();

            // Cloudinary detecta automáticamente si es imagen, vídeo o raw (docs)
            UploadResult result;
            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            var esVideo   = new[] { ".mp4", ".mov", ".avi", ".mkv", ".webm" }.Contains(extension);
            var esImagen  = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" }.Contains(extension);

            if (esImagen)
            {
                var uploadParams = new ImageUploadParams
                {
                    File           = new FileDescription(archivo.FileName, stream),
                    Folder         = "timelapse/imagenes",
                    UseFilename    = false,
                    UniqueFilename = true
                };
                result = await _cloudinary.UploadAsync(uploadParams);
            }
            else if (esVideo)
            {
                var uploadParams = new VideoUploadParams
                {
                    File           = new FileDescription(archivo.FileName, stream),
                    Folder         = "timelapse/videos",
                    UseFilename    = false,
                    UniqueFilename = true
                };
                result = await _cloudinary.UploadAsync(uploadParams);
            }
            else
            {
                // PDFs, docs, etc. → tipo "raw"
                var uploadParams = new RawUploadParams
                {
                    File           = new FileDescription(archivo.FileName, stream),
                    Folder         = "timelapse/documentos",
                    UseFilename    = false,
                    UniqueFilename = true
                };
                result = await _cloudinary.UploadAsync(uploadParams);
            }

            if (result.Error != null)
                throw new Exception($"Error al subir a Cloudinary: {result.Error.Message}");

            return result.SecureUrl.ToString();
        }

        public async Task DeleteAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentException("El PublicId no puede estar vacío.");

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}