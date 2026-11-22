using CineApp.Back.Data;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineApp.Back.Repositories.Implementations;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly CineDbContext _context;

    public UsuarioRepository(CineDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByNombreUsuario(string nombreUsuario)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
    }
}
