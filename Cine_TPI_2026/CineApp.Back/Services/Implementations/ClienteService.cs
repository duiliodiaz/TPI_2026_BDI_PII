using CineApp.Back.DTOs;
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
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            this._repository = repository;
        }

        public async Task<(bool Exito, string Mensaje, ClienteDTO? NewClient)> ActualizarClienteAsync(int id, ClienteDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del cliente son nulos.", null);

            var clienteExistente = await _repository.GetClienteByIdAsync(id);

            if (clienteExistente == null)
                return (false, $"No se encontró un cliente activo con ID '{id}'.", null);

            if (await _repository.ExistsDniAsync(dto.Dni, id))
                return (false, $"Ya existe otro cliente activo registrado con el DNI '{dto.Dni}'.", null);

            if (await _repository.ExistsEmailAsync(dto.EMail, id))
                return (false, $"El email '{dto.EMail.Trim()}' ya pertenece a otro cliente.", null);

            clienteExistente.Nombre = dto.Nombre.Trim();
            clienteExistente.Apellido = dto.Apellido.Trim();
            clienteExistente.Dni = dto.Dni;
            clienteExistente.EMail = dto.EMail;
            clienteExistente.Activo = dto.Activo;

            bool updated = await _repository.UpdateClienteAsync(clienteExistente);

            if (!updated)
                return (false, $"Ocurrió un error al intentar actualizar el cliente '{clienteExistente.Nombre}, {clienteExistente.Apellido}'.", null);

            return (true, "Cliente actualizado exitosamente.", dto);
        }

        public async Task<(bool Exito, string Mensaje, ClienteDTO? CreatedClient)> AñadirClienteAsync(ClienteDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del cliente son nulos.", null);

            if (await _repository.ExistsDniAsync(dto.Dni))
                return (false, $"Ya existe un cliente activo registrado con el DNI '{dto.Dni}'.", null);

            if (await _repository.ExistsEmailAsync(dto.EMail))
                return (false, $"El email '{dto.EMail?.Trim()}' ya está registrado por otro cliente.", null);

            var nuevoCliente = new Cliente
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = dto.Dni,
                EMail = dto.EMail,
                Activo = true
            };

            bool created = await _repository.AddClienteAsync(nuevoCliente);

            if (!created)
                return (false, $"Ocurrió un error al intentar crear el cliente '{nuevoCliente.Nombre}, {nuevoCliente.Apellido}'.", null);

            var createdClientDto = MapClientToDTO(nuevoCliente);

            return (true, "Cliente creado exitosamente.", createdClientDto);
        }

        public Task<bool> EliminarClienteAsync(int id)
        {
            return _repository.DeleteClienteAsync(id);
        }

        public async Task<List<ClienteDTO>> ObtenerTodosClientesAsync(bool? activo, long? dni)
        {
            var lstClientes = await _repository.GetAllClientesAsync(activo, dni);

            return lstClientes.Select(c => MapClientToDTO(c)).ToList();
        }

        public async Task<ClienteDTO?> ObtenerClientePorIdAsync(int id)
        {
            var cliente = await _repository.GetClienteByIdAsync(id);

            if (cliente == null)
                return null;

            return MapClientToDTO(cliente);
        }

        private ClienteDTO MapClientToDTO(Cliente cliente)
        {
            return new ClienteDTO
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                EMail = cliente.EMail,
                Activo = cliente.Activo
            };
        }
    }
}
