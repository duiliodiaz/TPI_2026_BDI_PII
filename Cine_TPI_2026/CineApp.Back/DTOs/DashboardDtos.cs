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
