using CineApp.Back.DTOs;
using CineApp.Back.Services.Implementations;
using CineApp.Back.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        private readonly IComprobanteService _service;

        public ComprobantesController(IComprobanteService comprobanteService)
        {
            _service = comprobanteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComprobantes()
        {
            var comprobantes = await _service.ObtenerTodosComprobantesAsync();

            return Ok(comprobantes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComprobanteById(int id)
        {
            var comprobante = await _service.ObtenerComprobantePorIdAsync(id);

            if (comprobante == null)
                return NotFound("Comprobante no encontrado.");

            return Ok(comprobante);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComprobante([FromBody] ComprobanteRequestDTO dto)
        {
            var creado = await _service.AñadirComprobanteAsync(dto);

            if (!creado.Exito)
                return BadRequest(new { mensaje = creado.Mensaje });

            int id = await _service.ObtenerIdNuevoComprobanteAsync();

            return Ok(new { mensaje = creado.Mensaje, id = id, data = dto });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComprobante(int id, [FromBody] ComprobanteRequestDTO dto)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "El ID del comprobante debe ser mayor a cero." });

            var actualizado = await _service.ActualizarComprobanteAsync(id, dto);

            if (!actualizado.Exito)
                return BadRequest(new { mensaje = actualizado.Mensaje });

            return Ok(new { mensaje = actualizado.Mensaje, id = id, data = dto});
        }

        // DELETE: api/Comprobantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComprobante(int id)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "El ID del comprobante debe ser mayor a cero." });

            bool eliminado = await _service.EliminarComprobanteAsync(id);

            if (!eliminado)
                return NotFound("Comprobante no encontrado.");

            return Ok(new { mensaje = "Comprobante dado de baja exitosamente." });
        }

        [HttpGet("formas-pago")]
        public async Task<IActionResult> GetAllFormasPago()
        {
            var formasPago = await _service.ObtenerTodasFormasPagoAsync();

            // evitando bucle infinito con la litsa de Comprobantes
            var resultado = formasPago.Select(f => new { f.IdFormaPago, f.Descripcion });

            return Ok(resultado);
        }
    }
}
