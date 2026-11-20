using CineApp.Back.DTOs;

namespace CineApp.Back.Services.Interface;

public interface IDashboardService
{
    Task<List<FechaEntradasDto>> ObtenerFechasMasEntradas(int top);

    Task<List<ProductoVendidoDto>> ObtenerProductosMasVendidos(int top);
}
