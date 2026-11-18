using CineApp.Back.DTOs;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Implementations;
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
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            this._repository = repository;
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarProductoAsync(int id, ProductoDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del producto son nulos.");

            var productoExistente = await _repository.GetProductoByIdAsync(id);

            if (productoExistente == null)
                return (false, $"No se encontró un producto activo con ID '{id}'.");

            if (!await _repository.ExistsTipoProductoAsync(dto.IdTipoProducto))
                return (false, $"El tipo de producto con ID '{dto.IdTipoProducto}' no existe.");

            if (!await _repository.ExistsTamañoProductoAsync(dto.IdTamañoProducto))
                return (false, $"El tamaño de producto con ID '{dto.IdTamañoProducto}' no existe.");

            if (await _repository.ExistsDescripcionAsync(dto.Descripcion, id))
                return (false, $"Ya existe otro producto activo llamado '{dto.Descripcion.Trim()}'.");

            productoExistente.Descripcion = dto.Descripcion.Trim();
            productoExistente.PreUnitario = dto.PreUnitario;
            productoExistente.IdTipoProducto = dto.IdTipoProducto;
            productoExistente.IdTamañoProducto = dto.IdTamañoProducto;
            productoExistente.Activo = dto.Activo;

            bool actualizado = await _repository.UpdateProductoAsync(productoExistente);

            if (actualizado)
                return (true, "Producto actualizado exitosamente.");
            else
                return (false, $"Ocurrió un error al intentar actualizar el producto ID: '{productoExistente.IdProducto}' - Nombre: '{productoExistente.Descripcion}'.");
        }

        public async Task<(bool Exito, string Mensaje)> AñadirProductoAsync(ProductoDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del producto son nulos.");

            if (!await _repository.ExistsTipoProductoAsync(dto.IdTipoProducto))
                return (false, $"El tipo de producto con ID '{dto.IdTipoProducto}' no existe.");

            if (!await _repository.ExistsTamañoProductoAsync(dto.IdTamañoProducto))
                return (false, $"El tamaño de producto con ID '{dto.IdTamañoProducto}' no existe.");

            if (await _repository.ExistsDescripcionAsync(dto.Descripcion))
                return (false, $"Ya existe un producto activo llamado '{dto.Descripcion.Trim()}'.");

            var nuevoProducto = new Producto
            {
                Descripcion = dto.Descripcion.Trim(),
                PreUnitario = dto.PreUnitario,
                IdTipoProducto = dto.IdTipoProducto,
                IdTamañoProducto = dto.IdTamañoProducto,
                Activo = true
            };

            bool guardado = await _repository.AddProductoAsync(nuevoProducto);

            if (guardado)
                return (true, "Producto creado exitosamente.");
            else
                return (false, $"Ocurrió un error al intentar crear el producto '{nuevoProducto.IdProducto}' - '{nuevoProducto.Descripcion}'.");
        }

        public Task<bool> EliminarProductoAsync(int id)
        {
            return _repository.DeleteProductoAsync(id);
        }

        public async Task<ProductoResponseDTO?> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _repository.GetProductoByIdAsync(id);

            if (producto == null)
                return null;

            return MapearProductoHaciaDTO(producto);
        }

        public async Task<List<ProductoResponseDTO>> ObtenerTodosProductosAsync()
        {
            var lstProductos = await _repository.GetAllProductosAsync();

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
