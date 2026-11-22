using CineApp.Back.DTOs;
using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoResponseDTO>> ObtenerTodosProductosAsync(bool? activo, int? tipo, string? nombre);

        Task<ProductoResponseDTO?> ObtenerProductoPorIdAsync(int id);

        Task<(bool Exito, string Mensaje, ProductoResponseDTO? CreatedProduct, TipoError Error)> AñadirProductoAsync(ProductoRequestDTO dto);

        Task<(bool Exito, string Mensaje, ProductoResponseDTO? NewProduct, TipoError Error)> ActualizarProductoAsync(int id, ProductoRequestDTO dto);

        Task<(bool Exito, string Mensaje, ProductoResponseDTO? DeletedProduct, TipoError Error)> EliminarProductoAsync(int id);
    }
}
