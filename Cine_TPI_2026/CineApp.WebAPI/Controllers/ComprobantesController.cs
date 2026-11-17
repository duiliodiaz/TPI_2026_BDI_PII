using CineApp.Back.DTOs.Comprobantes;
using CineApp.Back.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        private readonly IComprobanteService _comprobanteService;

        public ComprobantesController(IComprobanteService comprobanteService)
        {
            _comprobanteService = comprobanteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComprobantes()
        {
            var comprobantes = await _comprobanteService.ObtenerTodosComprobantesAsync();

            return Ok(comprobantes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComprobanteById(int id)
        {
            var comprobante = await _comprobanteService.ObtenerComprobantePorIdAsync(id);

            if (comprobante == null)
            {
                return NotFound("Comprobante no encontrado.");
            }

            return Ok(comprobante);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComprobante([FromBody] ComprobanteRequestDTO dto)
        {
            bool creado = await _comprobanteService.AñadirComprobanteAsync(dto);

            if (!creado)
            {
                return BadRequest("No se pudo registrar el comprobante. Verifique cliente, forma de pago y detalles.");
            }

            return Ok(new { mensaje = "Comprobante registrado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComprobante(int id, [FromBody] ComprobanteRequestDTO dto)
        {
            bool actualizado = await _comprobanteService.ActualizarComprobanteAsync(id, dto);

            if (!actualizado)
            {
                return NotFound("No se encontró el comprobante activo o los datos son inválidos.");
            }

            return Ok(new { mensaje = "Comprobante actualizado exitosamente." });
        }

        // DELETE: api/Comprobantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComprobante(int id)
        {
            bool eliminado = await _comprobanteService.EliminarComprobanteAsync(id);

            if (!eliminado)
            {
                return NotFound("Comprobante no encontrado.");
            }

            return Ok(new { mensaje = "Comprobante dado de baja exitosamente." });
        }

        [HttpGet("formas-pago")]
        public async Task<IActionResult> GetAllFormasPago()
        {
            var formasPago = await _comprobanteService.ObtenerTodasFormasPagoAsync();

            // evitando bucle infinito con la litsa de Comprobantes
            var resultado = formasPago.Select(f => new { f.IdFormaPago, f.Descripcion });

            return Ok(resultado);
        }
    }
}
