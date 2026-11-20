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
        Task<List<ProductoResponseDTO>> ObtenerTodosProductosAsync();

        Task<ProductoResponseDTO?> ObtenerProductoPorIdAsync(int id);

        Task<(bool Exito, string Mensaje)> AñadirProductoAsync(ProductoRequestDTO dto);

        Task<(bool Exito, string Mensaje)> ActualizarProductoAsync(int id, ProductoRequestDTO dto);

        Task<bool> EliminarProductoAsync(int id);
    }
}
