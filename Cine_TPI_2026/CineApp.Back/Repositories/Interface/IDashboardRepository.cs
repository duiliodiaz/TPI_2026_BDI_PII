using CineApp.Back.DTOs;

namespace CineApp.Back.Repositories.Interface;

public interface IDashboardRepository
{
    Task<List<FechaEntradasDto>> GetFechasMasEntradas(int top);

    Task<List<ProductoVendidoDto>> GetProductosMasVendidos(int top);
}
