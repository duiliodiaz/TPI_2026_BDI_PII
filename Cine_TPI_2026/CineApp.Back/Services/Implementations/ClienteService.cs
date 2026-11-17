using CineApp.Back.DTOs.Clientes;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Implementations;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            this._clienteRepository = clienteRepository;
        }

        public async Task<bool> ActualizarClienteAsync(int id, ClienteDTO dto)
        {
            // validaciones

            var cliente = await _clienteRepository.GetClienteByIdAsync(id);

            if (cliente == null)
            {
                return false;
            }

            cliente.Nombre = dto.Nombre.Trim();
            cliente.Apellido = dto.Apellido.Trim();
            cliente.Dni = dto.Dni;
            cliente.EMail = dto.EMail;

            return await _clienteRepository.UpdateClienteAsync(cliente);
        }

        public async Task<bool> AñadirClienteAsync(ClienteDTO dto)
        {
            // validaciones

            var nuevoCliente = new Cliente
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = dto.Dni,
                EMail = dto.EMail,
                Activo = true
            };

            return await _clienteRepository.AddClienteAsync(nuevoCliente);
        }

        public Task<bool> EliminarClienteAsync(int id)
        {
            return _clienteRepository.DeleteClienteAsync(id);
        }

        public Task<List<Cliente>> ObtenerTodosClientesAsync()
        {
            return _clienteRepository.GetAllClientesAsync();
        }

        public Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return _clienteRepository.GetClienteByIdAsync(id);
        }
    }
}
