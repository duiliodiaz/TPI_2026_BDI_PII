using CineApp.Back.Models;

namespace CineApp.Back.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByNombreUsuario(string nombreUsuario);
}
