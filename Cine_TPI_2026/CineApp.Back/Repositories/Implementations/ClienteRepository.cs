using CineApp.Back.Data;
using CineApp.Back.DTOs.Clientes;
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
    public class ClienteRepository : IClienteRepository
    {
        private readonly CineDbContext _context;

        public ClienteRepository(CineDbContext context)
        {
            this._context = context;
        }

        public async Task<bool> AddClienteAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            // borrado lógico:

            var cliente = await GetClienteByIdAsync(id);

            if (cliente != null)
            {
                cliente.Activo = false;
                int filasAfectadas = await _context.SaveChangesAsync();
                return filasAfectadas > 0;
            }

            return false;
        }

        public async Task<List<Cliente>> GetAllClientesAsync()
        {
            return await _context.Clientes
                .Where(c => c.Activo)
                .ToListAsync();
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            return await _context.Clientes
                .Where(c => c.Activo)
                .Where(c => c.IdCliente == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
