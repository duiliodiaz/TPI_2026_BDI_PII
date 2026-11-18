using CineApp.Back.DTOs;
using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAllClientesAsync();

        Task<Cliente?> GetClienteByIdAsync(int id);

        Task<bool> AddClienteAsync(Cliente cliente);

        Task<bool> UpdateClienteAsync(Cliente cliente);

        Task<bool> DeleteClienteAsync(int id);

        // métodos auxiliares para validaciones

        Task<bool> ExistsDniAsync(long dni, int? idExcluir = null);

        Task<bool> ExistsEmailAsync(string email, int? idExcluir = null);
    }
}
