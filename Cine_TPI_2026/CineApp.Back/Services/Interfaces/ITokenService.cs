using CineApp.Back.Models;

namespace CineApp.Back.Services.Interfaces;

public interface ITokenService
{
    (string Token, DateTime Expira) GenerarToken(Usuario usuario);
}
