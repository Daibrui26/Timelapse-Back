using System;

namespace TimelapseAPI.Models
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public string Texto { get; set; } = string.Empty;
        public DateTime FechaComentario { get; set; }
        public int IdUsuario { get; set; }
        public int IdCapsula { get; set; }
    }
}