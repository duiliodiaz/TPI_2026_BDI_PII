using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Interfaces
{
    public interface ICatalogoProductoService
    {
        Task<List<TiposProducto>> ObtenerTodosTiposProductoAsync();

        Task<List<TamañosProducto>> ObtenerTodosTamañosProductoAsync();
    }
}
