using CineApp.Back.Data;
using CineApp.Back.DTOs;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<List<Producto>> GetAllProductosAsync(bool? activo, int? tipo, string? nombre)
        {
            var query = _context.Productos
                .Include(p => p.IdTipoProductoNavigation)
                .Include(p => p.IdTamañoProductoNavigation)
                .AsQueryable();

            if (activo.HasValue)
            {
                if (activo.Value)
                    query = query.Where(p => p.Activo);
                else
                    query = query.Where(p => !p.Activo);
            }

            if (!activo.HasValue || activo.Value) // solo para "todos" o "activos"
            {
                if (tipo.HasValue) // si tipo true o false
                    query = query.Where(p => p.IdTipoProducto == tipo.Value);

                if (!string.IsNullOrWhiteSpace(nombre)) // si nombre no es null
                    query = query.Where(p => p.Descripcion.Contains(nombre.Trim())); // contains = like (sql)
            }

            return await query.ToListAsync();
        }

        public Task<Producto?> GetProductoByIdAsync(int id)
        {
            return _context.Productos
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

        public async Task<bool> ExistsProductoAsync(string descripcion, int idTamaño, int? idExcluir = null)
        { // verifica si exsite la combinacion nombre + tamaño, siempre buscando un producto activo,
            return await _context.Productos.AnyAsync(p => p.Activo &&
                p.Descripcion.ToLower() == descripcion.Trim().ToLower() &&
                p.IdTamañoProducto == idTamaño &&
                (!idExcluir.HasValue || p.IdProducto != idExcluir.Value)); // idExcluir: en el update, ignora el propio producto
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
