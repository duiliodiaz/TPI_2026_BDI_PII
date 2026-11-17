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
    public class CatalogoProductoService : ICatalogoProductoService
    {
        private readonly ITamañoProductoRepository _tamañoProductoRepository;
        private readonly ITipoProductoRepository _tipoProductoRepository;

        public CatalogoProductoService(ITamañoProductoRepository tamañoProductoRepository,
            ITipoProductoRepository tipoProductoRepository)
        {
            this._tamañoProductoRepository = tamañoProductoRepository;
            this._tipoProductoRepository = tipoProductoRepository;
        }
        
        public Task<List<TamañosProducto>> ObtenerTodosTamañosProductoAsync()
        {
            return _tamañoProductoRepository.GetAllTamañosProductosAsync();
        }

        public Task<List<TiposProducto>> ObtenerTodosTiposProductoAsync()
        {
            return _tipoProductoRepository.GetAllTiposProductosAsync();
        }
    }
}
