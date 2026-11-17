using CineApp.Back.DTOs.Clientes;
using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> ObtenerTodosClientesAsync();

        Task<Cliente?> ObtenerClientePorIdAsync(int id);

        Task<bool> AñadirClienteAsync(ClienteDTO dto);

        Task<bool> ActualizarClienteAsync(int id, ClienteDTO dto);

        Task<bool> EliminarClienteAsync(int id);
    }
}
