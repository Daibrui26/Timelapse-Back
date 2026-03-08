using System.ComponentModel.DataAnnotations;

namespace TimelapseAPI.Models.DTOs
{
    public class ContenidoArchivoCreateDTO
    {
        [Required]
        public int IdCapsula { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe proporcionar un archivo.")]
        public IFormFile Archivo { get; set; } = null!;
    }
}