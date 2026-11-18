using CineApp.Back.Data;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Repositories.Implementations
{
    public class ComprobanteRepository : IComprobanteRepository
    {
        private readonly CineDbContext _context;

        public ComprobanteRepository(CineDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddComprobanteAsync(Comprobante comprobante)
        {
            await _context.Comprobantes.AddAsync(comprobante);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteComprobanteAsync(int id)
        {
            var comprobante = await GetComprobanteByIdAsync(id);

            if (comprobante == null)
            {
                return false;
            }

            comprobante.Activo = false;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Comprobante>> GetAllComprobantesAsync()
        {
            return await _context.Comprobantes
                .Where(c => c.Activo)
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdFormaPagoNavigation)
                .Include(c => c.DetallesComprobanteProductos)
                .ThenInclude(d => d.IdProductoNavigation)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<Comprobante?> GetComprobanteByIdAsync(int id)
        {
            return await _context.Comprobantes
                .Where(c => c.Activo && c.IdComprobante == id)
                .Include(c => c.IdClienteNavigation)
                .Include(c => c.IdFormaPagoNavigation)
                .Include(c => c.DetallesComprobanteProductos)
                .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateComprobanteAsync(Comprobante comprobante, List<DetallesComprobanteProducto> nuevosDetalles)
        {
            if (comprobante.DetallesComprobanteProductos != null && comprobante.DetallesComprobanteProductos.Count > 0)
            { // si tiene detalles los elimina
                _context.DetallesComprobanteProductos.RemoveRange(comprobante.DetallesComprobanteProductos);
            }

            comprobante.DetallesComprobanteProductos = nuevosDetalles;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
