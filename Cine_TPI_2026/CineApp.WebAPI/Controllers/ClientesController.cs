using CineApp.Back.DTOs;
using CineApp.Back.DTOs.Clientes;
using CineApp.Back.Models;
using CineApp.Back.Services.Implementations;
using CineApp.Back.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            this._clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            var lstClientes = await _clienteService.ObtenerTodosClientesAsync();

            return Ok(lstClientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorIdAsync(id);

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteDTO dto)
        {
            bool creado = await _clienteService.AñadirClienteAsync(dto);

            if (!creado)
            {
                return BadRequest("No se pudo crear el Cliente. Verifique los datos ingresados.");
            }

            return Ok(new { mensaje = "Cliente creado exitosamente" });
            //return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteDTO dto)
        {
            if (dto.IdCliente != 0 && id != dto.IdCliente)
            {
                return BadRequest("El ID de la URL no coincide con el del cuerpo de la petición.");
            }

            bool actualizado = await _clienteService.ActualizarClienteAsync(id, dto);

            if (!actualizado)
            {
                return NotFound("No se encontró el cliente activo o los datos son inválidos.");
            }

            return Ok(new { mensaje = "Cliente actualizado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            bool eliminado = await _clienteService.EliminarClienteAsync(id);

            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
