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

        public async Task<(bool Exito, string Mensaje, ClienteDTO? NewClient, TipoError Error)> ActualizarClienteAsync(int id, ClienteRequestDTO dto)
        {
            //if (dto == null)
            //    return (false, "Los datos del cliente son nulos.", null, TipoError.Validacion);

            var clienteExistente = await _repository.GetClienteByIdAsync(id);

            if (clienteExistente == null)
                return (false, $"No se encontró un cliente activo con ID '{id}'.", null, TipoError.NoEncontrado);

            if (await _repository.ExistsDniAsync(dto.Dni, id))
                return (false, $"Ya existe otro cliente activo registrado con el DNI '{dto.Dni}'.", null, TipoError.Conflicto);

            if (await _repository.ExistsEmailAsync(dto.EMail, id))
                return (false, $"El email '{dto.EMail.Trim()}' ya pertenece a otro cliente.", null, TipoError.Conflicto);

            clienteExistente.Nombre = dto.Nombre.Trim();
            clienteExistente.Apellido = dto.Apellido.Trim();
            clienteExistente.Dni = dto.Dni;
            clienteExistente.EMail = dto.EMail;

            // solo cambia el estado si lo mandan explícitamente
            if (dto.Activo.HasValue)
                clienteExistente.Activo = dto.Activo.Value;

            bool updated = await _repository.UpdateClienteAsync(clienteExistente);

            if (!updated)
                return (false, $"Ocurrió un error al intentar actualizar el cliente '{clienteExistente.Nombre}, {clienteExistente.Apellido}'.", null, TipoError.Interno);

            return (true, "Cliente actualizado exitosamente.", MapClientToDTO(clienteExistente), TipoError.Ninguno);
        }

        public async Task<(bool Exito, string Mensaje, ClienteDTO? CreatedClient, TipoError Error)> AñadirClienteAsync(ClienteRequestDTO dto)
        {
            //if (dto == null)
            //    return (false, "Los datos del cliente son nulos.", null, TipoError.Validacion);

            if (await _repository.ExistsDniAsync(dto.Dni))
                return (false, $"Ya existe un cliente activo registrado con el DNI '{dto.Dni}'.", null, TipoError.Conflicto);

            if (await _repository.ExistsEmailAsync(dto.EMail))
                return (false, $"El email '{dto.EMail?.Trim()}' ya está registrado por otro cliente.", null, TipoError.Conflicto);

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
                return (false, $"Ocurrió un error al intentar crear el cliente '{nuevoCliente.Nombre}, {nuevoCliente.Apellido}'.", null, TipoError.Interno);

            var createdClientDto = MapClientToDTO(nuevoCliente);

            return (true, "Cliente creado exitosamente.", createdClientDto, TipoError.Ninguno);
        }

        public async Task<(bool Exito, string Mensaje, ClienteDTO? DeletedClient, TipoError Error)> EliminarClienteAsync(int id)
        {
            var deletedClient = await ObtenerClientePorIdAsync(id);

            if (deletedClient == null)
                return (false, $"El cliente no existe en la base de datos.", null, TipoError.NoEncontrado);

            if (!deletedClient.Activo)
                return (false, $"El cliente '{deletedClient.Nombre}, {deletedClient.Apellido}' ya está dado de baja.", null, TipoError.Conflicto);

            bool deleted = await _repository.DeleteClienteAsync(id);

            if (!deleted)
                return (false, $"Ocurrió un error al intentar eliminar el cliente '{deletedClient.Nombre}, {deletedClient.Apellido}'.", null, TipoError.Interno);

            return (true, "Cliente eliminado exitosamente.", deletedClient, TipoError.Ninguno);
        }

        public async Task<List<ClienteDTO>> ObtenerTodosClientesAsync(bool? activo, long? dni)
        {
            var lstClientes = await _repository.GetAllClientesAsync(activo, dni);

            return (lstClientes.Select(c => MapClientToDTO(c)).ToList());
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
