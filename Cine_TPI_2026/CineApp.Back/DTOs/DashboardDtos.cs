namespace CineApp.Back.DTOs;

public class FechaEntradasDto
{
    public DateTime Fecha { get; set; }

    public int CantidadEntradas { get; set; }

    public decimal TotalRecaudado { get; set; }
}

public class ProductoVendidoDto
{
    public int IdProducto { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Tamaño { get; set; } = null!;

    public int CantidadVendida { get; set; }

    public int CantidadVentas { get; set; }

    public decimal TotalRecaudado { get; set; }
}

public class ResumenDashboardDto
{
    public int CantidadEntradasVendidas { get; set; }

    public decimal IngresosEntradas { get; set; }

    public decimal IngresosProductos { get; set; }

    public decimal IngresosTotales { get; set; }

    public int ClientesActivos { get; set; }
}

public class IngresoDiarioDto
{
    public DateTime Fecha { get; set; }

    public string DiaSemana { get; set; } = null!;

    public decimal IngresoEntradas { get; set; }

    public decimal IngresoProductos { get; set; }

    public decimal IngresoTotal { get; set; }
}

public class IngresosSemanaDto
{
    public DateTime Desde { get; set; }

    public DateTime Hasta { get; set; }

    public decimal TotalSemana { get; set; }

    public List<IngresoDiarioDto> Dias { get; set; } = new();

    // Día con mayor ingreso de la semana; null si la semana no tuvo ventas.
    public IngresoDiarioDto? Pico { get; set; }
}
