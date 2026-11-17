namespace CineApp.Back.DTOs;

public class LoginRequestDto
{
    public string NombreUsuario { get; set; } = null!;

    public string Password { get; set; } = null!;
}

public class LoginResponseDto
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string? Email { get; set; }

    public string Rol { get; set; } = null!;
}
