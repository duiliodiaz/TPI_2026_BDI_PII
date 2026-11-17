using CineApp.Back.Data;
using CineApp.Back.DTOs;
using CineApp.Back.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CineApp.Back.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly CineDbContext _context;

    public DashboardRepository(CineDbContext context)
    {
        _context = context;
    }

    public async Task<List<FechaEntradasDto>> GetFechasMasEntradas(int top)
    {
        return await _context.Entradas
            .GroupBy(e => e.IdFuncionNavigation.Fecha.Date)
            .Select(g => new FechaEntradasDto
            {
                Fecha = g.Key,
                CantidadEntradas = g.Count(),
                TotalRecaudado = g.Sum(e => e.PrecioCobrado)
            })
            .OrderByDescending(x => x.CantidadEntradas)
            .ThenByDescending(x => x.TotalRecaudado)
            .Take(top)
            .ToListAsync();
    }

    public async Task<List<ProductoVendidoDto>> GetProductosMasVendidos(int top)
    {
        return await _context.DetallesComprobanteProductos
            .GroupBy(d => new
            {
                d.IdProducto,
                d.IdProductoNavigation.Descripcion,
                Tamaño = d.IdProductoNavigation.IdTamañoProductoNavigation.Descripcion
            })
            .Select(g => new ProductoVendidoDto
            {
                IdProducto = g.Key.IdProducto,
                Descripcion = g.Key.Descripcion,
                Tamaño = g.Key.Tamaño,
                CantidadVendida = g.Sum(d => d.CantidadProductos),
                CantidadVentas = g.Count(),
                TotalRecaudado = g.Sum(d => d.CantidadProductos * d.PrecioUnitario)
            })
            .OrderByDescending(x => x.CantidadVendida)
            .ThenByDescending(x => x.TotalRecaudado)
            .Take(top)
            .ToListAsync();
    }
}
