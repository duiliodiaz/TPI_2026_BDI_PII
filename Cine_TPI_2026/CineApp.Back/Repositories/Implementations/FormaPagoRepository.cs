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
    public class FormaPagoRepository : IFormaPagoRepository
    {
        private readonly CineDbContext _context;

        public FormaPagoRepository(CineDbContext context)
        {
            _context = context;
        }

        public async Task<List<FormasPago>> GetAllFormasPagoAsync()
        {
            return await _context.FormasPagos.ToListAsync();
        }
    }
}
