using CineApp.Back.DTOs.Clientes;
using CineApp.Back.DTOs.Productos;
using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllProductosAsync();

        Task<Producto?> GetProductoByIdAsync(int id);

        Task<bool> AddProductoAsync(Producto producto);

        Task<bool> UpdateProductoAsync(Producto producto);

        Task<bool> DeleteProductoAsync(int id);
    }
}
