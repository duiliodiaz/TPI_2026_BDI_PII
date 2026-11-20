using CineApp.Back.DTOs;

namespace CineApp.Back.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<List<FechaEntradasDto>> GetFechasMasEntradas(int top);

    Task<List<ProductoVendidoDto>> GetProductosMasVendidos(int top);

    Task<int> CountEntradasVendidas();

    Task<decimal> SumIngresosEntradas();

    Task<decimal> SumIngresosProductos();

    Task<int> CountClientesActivos();

    // Ingresos agrupados por día (fecha del comprobante) dentro de [desde, hasta).
    Task<Dictionary<DateTime, decimal>> GetIngresosEntradasPorDia(DateTime desde, DateTime hasta);

    Task<Dictionary<DateTime, decimal>> GetIngresosProductosPorDia(DateTime desde, DateTime hasta);
}
