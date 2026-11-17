using CineApp.Back.Models;

namespace CineApp.Back.Repositories.Interface;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByNombreUsuario(string nombreUsuario);
}
