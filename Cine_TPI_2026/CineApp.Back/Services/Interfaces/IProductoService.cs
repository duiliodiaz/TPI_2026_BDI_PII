using CineApp.Back.DTOs.Clientes;
using CineApp.Back.DTOs.Productos;
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

        Task<bool> AñadirProductoAsync(ProductoDTO dto);

        Task<bool> ActualizarProductoAsync(int id, ProductoDTO dto);

        Task<bool> EliminarProductoAsync(int id);
    }
}
