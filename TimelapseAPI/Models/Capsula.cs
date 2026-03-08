using System;

namespace TimelapseAPI.Models
{
    public class Capsula
    {
        public int IdCapsula { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Visibilidad { get; set; } = string.Empty;
    }
}