using CineApp.Back.DTOs;
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
        Task<List<Producto>> GetAllProductosAsync(bool? activo, int? tipo, string? nombre);

        Task<Producto?> GetProductoByIdAsync(int id);

        Task<bool> AddProductoAsync(Producto producto);

        Task<bool> UpdateProductoAsync(Producto producto);

        Task<bool> DeleteProductoAsync(int id);

        // métodos auxiliares para validaciones

        Task<bool> ExistsDescripcionAsync(string descripcion, int? idExcluir = null);
        
        Task<bool> ExistsTipoProductoAsync(int idTipo);

        Task<bool> ExistsTamañoProductoAsync(int idTamaño);
    }
}
