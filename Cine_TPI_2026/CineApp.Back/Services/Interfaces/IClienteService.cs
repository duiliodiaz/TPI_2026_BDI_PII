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
        Task<List<ClienteDTO>> ObtenerTodosClientesAsync(bool? activo, long? dni);

        Task<ClienteDTO?> ObtenerClientePorIdAsync(int id);

        Task<(bool Exito, string Mensaje, ClienteDTO? CreatedClient, TipoError Error)> AñadirClienteAsync(ClienteRequestDTO dto);

        Task<(bool Exito, string Mensaje, ClienteDTO? NewClient, TipoError Error)> ActualizarClienteAsync(int id, ClienteRequestDTO dto);

        Task<(bool Exito, string Mensaje, ClienteDTO? DeletedClient, TipoError Error)> EliminarClienteAsync(int id);
    }
}
