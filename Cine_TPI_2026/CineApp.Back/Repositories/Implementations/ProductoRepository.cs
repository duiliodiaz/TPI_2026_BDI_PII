using CineApp.Back.Data;
using CineApp.Back.DTOs;
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
    public class ProductoRepository : IProductoRepository
    {
        private readonly CineDbContext _context;

        public ProductoRepository(CineDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> AddProductoAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await GetProductoByIdAsync(id);

            if (producto == null)
                return false;

            producto.Activo = false;

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<Producto>> GetAllProductosAsync()
        {
            return _context.Productos
                .Where(p => p.Activo)
                .Include(p => p.IdTipoProductoNavigation)
                .Include(p => p.IdTamañoProductoNavigation)
                .ToListAsync();
        }

        public Task<Producto?> GetProductoByIdAsync(int id)
        {
            return _context.Productos
                .Where(p => p.Activo)
                .Include(p => p.IdTipoProductoNavigation)
                .Include(p => p.IdTamañoProductoNavigation)
                .Where(p => p.IdProducto == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            _context.Productos.Update(producto);

            return await _context.SaveChangesAsync() > 0;
        }

        // Métodos auxiliaraes para validación en service

        public async Task<bool> ExistsDescripcionAsync(string descripcion, int? idExcluir = null)
        { // verifica si exsite la descripcion, siempre buscando un producto activo,
            return await _context.Productos.AnyAsync(p =>
                p.Activo &&
                p.Descripcion.ToLower() == descripcion.Trim().ToLower() &&
                (!idExcluir.HasValue || p.IdProducto != idExcluir.Value)); //idExcluir solo sirve para update
        }
        
        public async Task<bool> ExistsTipoProductoAsync(int idTipo)
        { // verifica que exista la clave foránea de tipo producto que recibe del dto
            return await _context.TiposProductos
                .AnyAsync(t => t.IdTipoProducto == idTipo);
        }

        public async Task<bool> ExistsTamañoProductoAsync(int idTamaño)
        { // clave foránea
            return await _context.TamañosProductos
                .AnyAsync(t => t.IdTamañoProducto == idTamaño);
        }
    }
}
