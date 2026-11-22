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


        public async Task<(bool Exito, string Mensaje)> ActualizarClienteAsync(int id, ClienteDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del cliente son nulos.");

            var clienteExistente = await _repository.GetClienteByIdAsync(id);

            if (clienteExistente == null)
                return (false, $"No se encontró un cliente activo con ID '{id}'.");

            if (await _repository.ExistsDniAsync(dto.Dni, id))
                return (false, $"Ya existe otro cliente activo registrado con el DNI '{dto.Dni}'.");

            if (await _repository.ExistsEmailAsync(dto.EMail, id))
                return (false, $"El email '{dto.EMail.Trim()}' ya pertenece a otro cliente.");

            clienteExistente.Nombre = dto.Nombre.Trim();
            clienteExistente.Apellido = dto.Apellido.Trim();
            clienteExistente.Dni = dto.Dni;
            clienteExistente.EMail = dto.EMail;
            clienteExistente.Activo = dto.Activo;

            bool actualizado = await _repository.UpdateClienteAsync(clienteExistente);

            if (actualizado)
                return (true, "Cliente actualizado exitosamente.");
            else
                return (false, $"Ocurrió un error al intentar actualizar el cliente '{clienteExistente.Nombre}, {clienteExistente.Apellido}'.");
        }

        public async Task<(bool Exito, string Mensaje)> AñadirClienteAsync(ClienteDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del cliente son nulos.");

            if (await _repository.ExistsDniAsync(dto.Dni))
                return (false, $"Ya existe un cliente activo registrado con el DNI '{dto.Dni}'.");

            if (await _repository.ExistsEmailAsync(dto.EMail))
                return (false, $"El email '{dto.EMail?.Trim()}' ya está registrado por otro cliente.");

            var nuevoCliente = new Cliente
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = dto.Dni,
                EMail = dto.EMail,
                Activo = true
            };

            bool guardado = await _repository.AddClienteAsync(nuevoCliente);

            if (guardado)
                return (true, "Cliente creado exitosamente.");
            else
                return (false, $"Ocurrió un error al intentar crear el cliente '{nuevoCliente.Nombre}, {nuevoCliente.Apellido}'.");
        }

        public Task<bool> EliminarClienteAsync(int id)
        {
            return _repository.DeleteClienteAsync(id);
        }

        public async Task<List<ClienteDTO>> ObtenerTodosClientesAsync()
        {
            var lstClientes = await _repository.GetAllClientesAsync();

            return lstClientes.Select(c => MapearClienteHaciaDTO(c)).ToList();
        }

        public async Task<ClienteDTO?> ObtenerClientePorIdAsync(int id)
        {
            var cliente = await _repository.GetClienteByIdAsync(id);

            if (cliente == null)
                return null;

            return MapearClienteHaciaDTO(cliente);
        }

        private ClienteDTO MapearClienteHaciaDTO(Cliente cliente)
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
