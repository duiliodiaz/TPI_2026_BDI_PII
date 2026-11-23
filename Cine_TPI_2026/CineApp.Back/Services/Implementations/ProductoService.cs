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

        public async Task<(bool Exito, string Mensaje, ProductoResponseDTO? NewProduct, TipoError Error)> ActualizarProductoAsync(int id, ProductoRequestDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del producto son nulos.", null, TipoError.Validacion);

            var productoExistente = await _repository.GetProductoByIdAsync(id);

            if (productoExistente == null)
                return (false, $"No se encontró un producto con ID '{id}'.", null, TipoError.NoEncontrado);

            if (dto.PreUnitario <= 0)
                return (false, $"El precio debe ser superior a $0", null, TipoError.Validacion);

            if (dto.PreUnitario > 10000000)
                return (false, $"El precio debe ser inferior a $10.000.000", null, TipoError.Validacion);

            if (!await _repository.ExistsTipoProductoAsync(dto.IdTipoProducto))
                return (false, $"El tipo de producto con ID '{dto.IdTipoProducto}' no existe.", null, TipoError.Validacion);

            if (!await _repository.ExistsTamañoProductoAsync(dto.IdTamañoProducto))
                return (false, $"El tamaño de producto con ID '{dto.IdTamañoProducto}' no existe.", null, TipoError.Validacion);

            if (await _repository.ExistsProductoAsync(dto.Descripcion, dto.IdTamañoProducto, id))
                return (false, $"Ya existe un producto activo '{dto.Descripcion.Trim()}' en ese tamaño.", null, TipoError.Conflicto);

            productoExistente.Descripcion = dto.Descripcion.Trim();
            productoExistente.PreUnitario = dto.PreUnitario;
            productoExistente.IdTipoProducto = dto.IdTipoProducto;
            productoExistente.IdTamañoProducto = dto.IdTamañoProducto;
            productoExistente.Activo = dto.Activo;

            bool updated = await _repository.UpdateProductoAsync(productoExistente);

            if (!updated)
                return (false, $"Ocurrió un error al intentar actualizar el producto ID: '{productoExistente.IdProducto}' - Nombre: '{productoExistente.Descripcion}'.", null, TipoError.Interno);

            var productoActualizado = await _repository.GetProductoByIdAsync(id);

            var dtoResponse = MapearProductoHaciaDTO(productoActualizado);

            return (true, "Producto actualizado exitosamente.", dtoResponse, TipoError.Ninguno);
        }

        public async Task<(bool Exito, string Mensaje, ProductoResponseDTO? CreatedProduct, TipoError Error)> AñadirProductoAsync(ProductoRequestDTO dto)
        {
            if (dto == null)
                return (false, "Los datos del producto son nulos.", null, TipoError.Validacion);

            if (!await _repository.ExistsTipoProductoAsync(dto.IdTipoProducto))
                return (false, $"El tipo de producto con ID '{dto.IdTipoProducto}' no existe.", null, TipoError.Validacion);

            if (!await _repository.ExistsTamañoProductoAsync(dto.IdTamañoProducto))
                return (false, $"El tamaño de producto con ID '{dto.IdTamañoProducto}' no existe.", null, TipoError.Validacion);

            if (await _repository.ExistsProductoAsync(dto.Descripcion, dto.IdTamañoProducto))
                return (false, $"Ya existe un producto activo '{dto.Descripcion.Trim()}' en ese tamaño.", null, TipoError.Conflicto);

            var nuevoProducto = new Producto
            {
                Descripcion = dto.Descripcion.Trim(),
                PreUnitario = dto.PreUnitario,
                IdTipoProducto = dto.IdTipoProducto,
                IdTamañoProducto = dto.IdTamañoProducto,
                Activo = true
            };

            bool guardado = await _repository.AddProductoAsync(nuevoProducto);

            if (!guardado)
                return (false, $"Ocurrió un error al intentar crear el producto '{nuevoProducto.Descripcion}'.", null, TipoError.Interno);

            var productoConRelaciones = await _repository.GetProductoByIdAsync(nuevoProducto.IdProducto);

            var createdProductDto = MapearProductoHaciaDTO(productoConRelaciones);

            return (true, "Producto creado exitosamente.", createdProductDto, TipoError.Ninguno);

        }

        public async Task<(bool Exito, string Mensaje, ProductoResponseDTO? DeletedProduct, TipoError Error)> EliminarProductoAsync(int id)
        {
            var deletedProduct = await ObtenerProductoPorIdAsync(id);

            if (deletedProduct == null)
                return (false, "El producto no existe en la base de datos.", null, TipoError.NoEncontrado);

            if (!deletedProduct.Activo)
                return (false, $"El producto '{deletedProduct.Descripcion}' ya está dado de baja.", null, TipoError.Conflicto);

            bool deleted = await _repository.DeleteProductoAsync(id);

            if (!deleted)
                return (false, $"Ocurrió un error al intentar eliminar el producto '{deletedProduct.Descripcion}'.", null, TipoError.Interno);

            return (true, "Producto eliminado exitosamente.", deletedProduct, TipoError.Ninguno);
        }

        public async Task<ProductoResponseDTO?> ObtenerProductoPorIdAsync(int id)
        {
            var producto = await _repository.GetProductoByIdAsync(id);

            if (producto == null)
                return null;

            return MapearProductoHaciaDTO(producto);
        }

        public async Task<List<ProductoResponseDTO>> ObtenerTodosProductosAsync(bool? activo, int? tipo, string? nombre)
        {
            var lstProductos = await _repository.GetAllProductosAsync(activo, tipo, nombre);

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
                TamañoProductoNombre = producto.IdTamañoProductoNavigation?.Descripcion ?? "Sin Tamaño",

                Activo = producto.Activo,
            };
        }
    }
}
