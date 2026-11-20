using CineApp.Back.DTOs;
using CineApp.Back.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    // GET: api/dashboard/fechas-mas-entradas?top=5
    [HttpGet("fechas-mas-entradas")]
    public async Task<ActionResult<List<FechaEntradasDto>>> GetFechasMasEntradas([FromQuery] int top = 5)
    {
        if (top <= 0)
            return BadRequest(new { mensaje = "El parámetro 'top' debe ser mayor a 0." });

        return Ok(await _service.ObtenerFechasMasEntradas(top));
    }

    // GET: api/dashboard/productos-mas-vendidos?top=5
    [HttpGet("productos-mas-vendidos")]
    public async Task<ActionResult<List<ProductoVendidoDto>>> GetProductosMasVendidos([FromQuery] int top = 5)
    {
        if (top <= 0)
            return BadRequest(new { mensaje = "El parámetro 'top' debe ser mayor a 0." });

        return Ok(await _service.ObtenerProductosMasVendidos(top));
    }
}
