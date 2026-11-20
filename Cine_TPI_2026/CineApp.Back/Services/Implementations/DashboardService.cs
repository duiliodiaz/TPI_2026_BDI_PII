using CineApp.Back.DTOs;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Services.Interfaces;

namespace CineApp.Back.Services.Implementations;

public class DashboardService : IDashboardService
{
    private static readonly string[] DiasSemana =
        { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

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

    public async Task<ResumenDashboardDto> ObtenerResumen()
    {
        var ingresosEntradas = await _repository.SumIngresosEntradas();
        var ingresosProductos = await _repository.SumIngresosProductos();

        return new ResumenDashboardDto
        {
            CantidadEntradasVendidas = await _repository.CountEntradasVendidas(),
            IngresosEntradas = ingresosEntradas,
            IngresosProductos = ingresosProductos,
            IngresosTotales = ingresosEntradas + ingresosProductos,
            ClientesActivos = await _repository.CountClientesActivos()
        };
    }

    public async Task<IngresosSemanaDto> ObtenerIngresosSemana(DateTime fecha)
    {
        // Sin zona horaria, para que el JSON muestre las fechas igual con o sin parámetro.
        var dia0 = DateTime.SpecifyKind(fecha.Date, DateTimeKind.Unspecified);

        // DayOfWeek arranca en domingo (0); se corre para que la semana empiece el lunes.
        var desde = dia0.AddDays(-(((int)dia0.DayOfWeek + 6) % 7));
        var hasta = desde.AddDays(7);

        var entradas = await _repository.GetIngresosEntradasPorDia(desde, hasta);
        var productos = await _repository.GetIngresosProductosPorDia(desde, hasta);

        var dias = Enumerable.Range(0, 7)
            .Select(i =>
            {
                var dia = desde.AddDays(i);
                var ingresoEntradas = entradas.GetValueOrDefault(dia);
                var ingresoProductos = productos.GetValueOrDefault(dia);

                return new IngresoDiarioDto
                {
                    Fecha = dia,
                    DiaSemana = DiasSemana[(int)dia.DayOfWeek],
                    IngresoEntradas = ingresoEntradas,
                    IngresoProductos = ingresoProductos,
                    IngresoTotal = ingresoEntradas + ingresoProductos
                };
            })
            .ToList();

        return new IngresosSemanaDto
        {
            Desde = desde,
            Hasta = hasta.AddDays(-1),
            TotalSemana = dias.Sum(d => d.IngresoTotal),
            Dias = dias,
            Pico = dias
                .Where(d => d.IngresoTotal > 0)
                .OrderByDescending(d => d.IngresoTotal)
                .FirstOrDefault()
        };
    }
}
