namespace TimelapseAPI.Models
{
    public class UsuarioCapsula
    {
        public int IdUsuarioCapsula { get; set; }
        public int IdUsuario { get; set; }
        public int IdCapsula { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}