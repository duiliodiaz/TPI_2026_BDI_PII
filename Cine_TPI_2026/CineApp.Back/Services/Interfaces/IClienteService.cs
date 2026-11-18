using CineApp.Back.DTOs;
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
        Task<List<ClienteDTO>> ObtenerTodosClientesAsync();

        Task<ClienteDTO?> ObtenerClientePorIdAsync(int id);

        Task<(bool Exito, string Mensaje)> AñadirClienteAsync(ClienteDTO dto);

        Task<(bool Exito, string Mensaje)> ActualizarClienteAsync(int id, ClienteDTO dto);

        Task<bool> EliminarClienteAsync(int id);
    }
}
