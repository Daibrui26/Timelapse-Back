namespace TimelapseAPI.Models.DTOs
{
    public class LoginRequestDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = "usuario";
    }
}