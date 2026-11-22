using CineApp.Back.DTOs;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Services.Interfaces;

namespace CineApp.Back.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly ITokenService _tokenService;

    public AuthService(IUsuarioRepository repository, ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto?> Login(LoginRequestDto request)
    {
        var usuario = await _repository.GetByNombreUsuario(request.NombreUsuario);

        if (usuario == null || usuario.Password != request.Password)
            return null;

        var (token, expira) = _tokenService.GenerarToken(usuario);

        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Token = token,
            Expira = expira
        };
    }
}
