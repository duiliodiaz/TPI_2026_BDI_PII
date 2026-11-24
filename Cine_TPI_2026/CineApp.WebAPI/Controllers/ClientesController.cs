using CineApp.Back.DTOs;
using CineApp.Back.Models;
using CineApp.Back.Services.Implementations;
using CineApp.Back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers
{
    [Authorize]
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

            if (dni < 1000000 || dni > 99999999)
                return BadRequest(new { mensaje = $"El DNI '{dni}' ingresado no es válido." }); // dni invalido
            else if (lstClientes.Count == 0)
                return NoContent();

            return Ok(lstClientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            var cliente = await _service.ObtenerClientePorIdAsync(id);

            if (cliente == null)
                return NotFound(new 
                {
                    mensaje = $"No se encontró un cliente activo con ID '{id}'."
                });

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteRequestDTO dto)
        {
            var creado = await _service.AñadirClienteAsync(dto);

            if (!creado.Exito)
                return this.RespuestaError(creado.Error, creado.Mensaje);

            return StatusCode(201, new
            {
                creado.Mensaje, creado.CreatedClient
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteRequestDTO dto)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "El ID debe ser mayor a cero." });
           
            var actualizado = await _service.ActualizarClienteAsync(id, dto);

            if (!actualizado.Exito)
                return this.RespuestaError(actualizado.Error, actualizado.Mensaje);

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
                return this.RespuestaError(eliminado.Error, eliminado.Mensaje);

            return Ok(new
            {
                eliminado.Mensaje, eliminado.DeletedClient
            });
        }
    }
}
