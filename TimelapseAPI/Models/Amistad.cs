namespace TimelapseAPI.Models
{
    public class Amistad
    {
        public int IdAmistad { get; set; }
        public int IdUsuario1 { get; set; }
        public int IdUsuario2 { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}