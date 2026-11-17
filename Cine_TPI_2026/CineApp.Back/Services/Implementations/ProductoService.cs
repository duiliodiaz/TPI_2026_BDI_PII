using CineApp.Back.DTOs.Clientes;
using CineApp.Back.DTOs.Productos;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            this._productoRepository = productoRepository;
        }

        public async Task<bool> ActualizarProductoAsync(int id, ProductoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Descripcion) || dto.PreUnitario <= 0)
            {
                return false;
            }

            var producto = await _productoRepository.GetProductoByIdAsync(id);

            if (producto == null)
            {
                return false;
            }

            producto.Descripcion = dto.Descripcion.Trim();
            producto.PreUnitario = dto.PreUnitario;
            producto.IdTipoProducto = dto.IdTipoProducto;
            producto.IdTamañoProducto = dto.IdTamañoProducto;

            return await _productoRepository.UpdateProductoAsync(producto);
        }

        public async Task<bool> AñadirProductoAsync(ProductoDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Descripcion) || dto.PreUnitario <= 0)
            {
                return false;
            }

            var nuevoProducto = new Producto
            {
                Descripcion = dto.Descripcion.Trim(),
                PreUnitario = dto.PreUnitario,
                IdTipoProducto = dto.IdTipoProducto,
                IdTamañoProducto = dto.IdTamañoProducto,
                Activo = true
            };

            return await _productoRepository.AddProductoAsync(nuevoProducto);
        }

        public Task<bool> EliminarProductoAsync(int id)
        {
            return _productoRepository.DeleteProductoAsync(id);
        }

        public async Task<ProductoResponseDTO?> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _productoRepository.GetProductoByIdAsync(id);

            if (producto == null)
            {
                return null;
            }

            return MapearProductoHaciaDTO(producto);
        }

        public async Task<List<ProductoResponseDTO>> ObtenerTodosProductosAsync()
        {
            var lstProductos = await _productoRepository.GetAllProductosAsync();

            return lstProductos.Select(p => MapearProductoHaciaDTO(p)).ToList();
        }

        private ProductoResponseDTO MapearProductoHaciaDTO(Producto producto)
        {
            return new ProductoResponseDTO
            {
                IdProducto = producto.IdProducto,
                Descripcion = producto.Descripcion,
                PreUnitario = producto.PreUnitario,
                IdTipoProducto = producto.IdTipoProducto,
                IdTamañoProducto = producto.IdTamañoProducto,

                TipoProductoNombre = producto.IdTipoProductoNavigation?.Descripcion ?? "Sin Tipo",
                TamañoProductoNombre = producto.IdTamañoProductoNavigation?.Descripcion ?? "Sin Tamaño"
            };
        }
    }
}
