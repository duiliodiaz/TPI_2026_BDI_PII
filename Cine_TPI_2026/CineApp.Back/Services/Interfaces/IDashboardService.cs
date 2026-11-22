using CineApp.Back.DTOs;

namespace CineApp.Back.Services.Interfaces;

public interface IDashboardService
{
    Task<List<FechaEntradasDto>> ObtenerFechasMasEntradas(int top);

    Task<List<ProductoVendidoDto>> ObtenerProductosMasVendidos(int top);

    Task<ResumenDashboardDto> ObtenerResumen();

    // Ingresos de lunes a domingo de la semana que contiene a 'fecha', con el día pico.
    Task<IngresosSemanaDto> ObtenerIngresosSemana(DateTime fecha);
}
