namespace TimelapseAPI.Services
{
    public interface IUploadService
    {
        ///Sube un archivo a Cloudinary y devuelve la URL pública.
        Task<string> UploadAsync(IFormFile archivo);

        ///Elimina un archivo de Cloudinary usando su PublicId.
        Task DeleteAsync(string publicId);
    }
}