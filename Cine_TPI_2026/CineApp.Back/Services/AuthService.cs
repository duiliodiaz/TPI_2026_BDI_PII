using CineApp.Back.DTOs;
using CineApp.Back.Repositories.Interface;
using CineApp.Back.Services.Interface;

namespace CineApp.Back.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repository;

    public AuthService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<LoginResponseDto?> Login(LoginRequestDto request)
    {
        var usuario = await _repository.GetByNombreUsuario(request.NombreUsuario);

        if (usuario == null || usuario.Password != request.Password)
            return null;

        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            Rol = usuario.Rol
        };
    }
}
