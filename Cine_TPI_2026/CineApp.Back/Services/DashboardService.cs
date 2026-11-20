using CineApp.Back.DTOs;
using CineApp.Back.Repositories.Interface;
using CineApp.Back.Services.Interface;

namespace CineApp.Back.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<FechaEntradasDto>> ObtenerFechasMasEntradas(int top)
    {
        return await _repository.GetFechasMasEntradas(top);
    }

    public async Task<List<ProductoVendidoDto>> ObtenerProductosMasVendidos(int top)
    {
        return await _repository.GetProductosMasVendidos(top);
    }
}
