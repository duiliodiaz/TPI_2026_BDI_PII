using CineApp.Back.DTOs;
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
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes(bool? activo, long? dni)
        {
            var lstClientes = await _service.ObtenerTodosClientesAsync(activo, dni);

            return Ok(lstClientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _service.ObtenerClientePorIdAsync(id);

            if (cliente == null)
                return NotFound(new { mensaje = $"No se encontró un cliente activo con ID '{id}'." });

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteDTO dto)
        {
            var creado = await _service.AñadirClienteAsync(dto);

            if (!creado.Exito)
                return BadRequest(new { mensaje = creado.Mensaje });

            return StatusCode(201, new
            {
                creado.Mensaje, creado.CreatedClient
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteDTO dto)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "El ID debe ser mayor a cero." });

            if (dto.IdCliente != 0 && id != dto.IdCliente)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del cliente enviado." });

            var actualizado = await _service.ActualizarClienteAsync(id, dto);

            if (!actualizado.Exito)
                return BadRequest(new { mensaje = actualizado.Mensaje });

            return Ok(new 
            {
                actualizado.Mensaje, actualizado.NewClient
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var eliminado = await _service.EliminarClienteAsync(id);

            if (!eliminado.Exito)
                return NotFound(new
                {
                    eliminado.Mensaje
                });

            return Ok(new
            {
                eliminado.Mensaje, eliminado.DeletedClient
            });
        }
    }
}
