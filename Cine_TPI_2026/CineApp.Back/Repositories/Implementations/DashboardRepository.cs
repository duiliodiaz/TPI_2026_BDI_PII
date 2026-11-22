using CineApp.Back.Data;
using CineApp.Back.DTOs;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CineApp.Back.Repositories.Implementations;

public class DashboardRepository : IDashboardRepository
{
    private readonly CineDbContext _context;

    public DashboardRepository(CineDbContext context)
    {
        _context = context;
    }

    // Entradas cobradas en un comprobante que no fue anulado.
    private IQueryable<Entrada> EntradasVendidas()
    {
        return _context.Entradas
            .Where(e => e.IdComprobanteNavigation != null && e.IdComprobanteNavigation.Activo);
    }

    // Productos de comprobantes que no fueron anulados.
    private IQueryable<DetallesComprobanteProducto> ProductosVendidos()
    {
        return _context.DetallesComprobanteProductos
            .Where(d => d.IdComprobanteNavigation.Activo);
    }

    public async Task<List<FechaEntradasDto>> GetFechasMasEntradas(int top)
    {
        return await _context.Entradas
            .Where(e => e.IdComprobanteNavigation == null || e.IdComprobanteNavigation.Activo)
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
        return await ProductosVendidos()
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

    public async Task<int> CountEntradasVendidas()
    {
        return await EntradasVendidas().CountAsync();
    }

    public async Task<decimal> SumIngresosEntradas()
    {
        return await EntradasVendidas().SumAsync(e => e.PrecioCobrado);
    }

    public async Task<decimal> SumIngresosProductos()
    {
        return await ProductosVendidos().SumAsync(d => d.CantidadProductos * d.PrecioUnitario);
    }

    public async Task<int> CountClientesActivos()
    {
        return await _context.Clientes.CountAsync(c => c.Activo);
    }

    public async Task<Dictionary<DateTime, decimal>> GetIngresosEntradasPorDia(DateTime desde, DateTime hasta)
    {
        return await EntradasVendidas()
            .Where(e => e.IdComprobanteNavigation!.Fecha >= desde && e.IdComprobanteNavigation.Fecha < hasta)
            .GroupBy(e => e.IdComprobanteNavigation!.Fecha.Date)
            .Select(g => new { Fecha = g.Key, Total = g.Sum(e => e.PrecioCobrado) })
            .ToDictionaryAsync(x => x.Fecha, x => x.Total);
    }

    public async Task<Dictionary<DateTime, decimal>> GetIngresosProductosPorDia(DateTime desde, DateTime hasta)
    {
        return await ProductosVendidos()
            .Where(d => d.IdComprobanteNavigation.Fecha >= desde && d.IdComprobanteNavigation.Fecha < hasta)
            .GroupBy(d => d.IdComprobanteNavigation.Fecha.Date)
            .Select(g => new { Fecha = g.Key, Total = g.Sum(d => d.CantidadProductos * d.PrecioUnitario) })
            .ToDictionaryAsync(x => x.Fecha, x => x.Total);
    }
}
