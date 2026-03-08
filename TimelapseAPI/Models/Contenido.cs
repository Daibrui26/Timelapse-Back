using System;

namespace TimelapseAPI.Models
{
    public class Contenido
    {
        public int IdContenido { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string? ContenidoTexto { get; set; }

        public string? UrlArchivo { get; set; }

        public string? PublicId { get; set; }

        public DateTime FechaSubida { get; set; }

        public int IdCapsula { get; set; }
    }
}