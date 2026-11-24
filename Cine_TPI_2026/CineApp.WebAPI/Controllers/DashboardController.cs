using CineApp.Back.DTOs;
using CineApp.Back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Administrador")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    // GET: api/dashboard/resumen
    [HttpGet("resumen")]
    public async Task<ActionResult<ResumenDashboardDto>> GetResumen()
    {
        return Ok(await _service.ObtenerResumen());
    }

    // GET: api/dashboard/ingresos-semana?fecha=2024-05-15
    // Sin fecha, devuelve la semana actual.
    [HttpGet("ingresos-semana")]
    public async Task<ActionResult<IngresosSemanaDto>> GetIngresosSemana([FromQuery] DateTime? fecha)
    {
        // Fechas extremas (0001, 9999) hacen fallar a SQL Server.
        if (fecha.HasValue && (fecha.Value.Year < 2000 || fecha.Value.Year > 2100))
            return BadRequest(new { mensaje = "La fecha debe estar entre los años 2000 y 2100." });

        return Ok(await _service.ObtenerIngresosSemana(fecha ?? DateTime.Today));
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
