using CineApp.Back.DTOs;

namespace CineApp.Back.Services.Interface;

public interface IAuthService
{
    // Devuelve null si el usuario no existe o la contraseña no coincide.
    Task<LoginResponseDto?> Login(LoginRequestDto request);
}
