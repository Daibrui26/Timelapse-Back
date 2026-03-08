using System;

namespace TimelapseAPI.Models
{
    public class Notificacion
    {
        public int IdNotificacion { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Leida { get; set; }
        public int IdUsuario { get; set; }
        public int? IdCapsula { get; set; }
    }
}