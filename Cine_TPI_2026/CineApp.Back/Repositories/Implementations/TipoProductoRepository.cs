using CineApp.Back.Data;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Repositories.Implementations
{
    public class TipoProductoRepository : ITipoProductoRepository
    {
        private readonly CineDbContext _context;

        public TipoProductoRepository(CineDbContext context)
        {
            this._context = context;
        }
        
        public Task<List<TiposProducto>> GetAllTiposProductosAsync()
        {
            return _context.TiposProductos
                .ToListAsync();
        }
    }
}
