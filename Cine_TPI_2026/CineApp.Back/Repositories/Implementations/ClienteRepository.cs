using CineApp.Back.Data;
using CineApp.Back.DTOs;
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
            var cliente = await GetClienteByIdAsync(id);

            if (cliente == null)
                return false;

            cliente.Activo = false;
            
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Cliente>> GetAllClientesAsync(bool? activo, long? dni)
        {
            var query = _context.Clientes
                .AsQueryable();

            if (activo.HasValue)
            {
                if (activo.Value)
                    query = query.Where(c => c.Activo);
                else
                    query = query.Where(c => !c.Activo);
            }

            if (!activo.HasValue || activo.Value) // para todos o activos
            {
                if (dni.HasValue)
                    query = query.Where(p => p.Dni == dni.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            return await _context.Clientes
                .Where(c => c.IdCliente == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);

            return await _context.SaveChangesAsync() > 0;
        }

        // métodos auxiliares para validaciones

        public async Task<bool> ExistsDniAsync(long dni, int? idExcluir = null)
        { // verifica si ya existe un cliente activo con ese dni
            return await _context.Clientes.AnyAsync(c =>
                c.Activo &&
                c.Dni == dni &&
                (!idExcluir.HasValue || c.IdCliente != idExcluir.Value)); // idExcluir solo sirve para update
        }

        public async Task<bool> ExistsEmailAsync(string email, int? idExcluir = null)
        { // erifica si ya existe un cliente activo con ese email
            if (string.IsNullOrEmpty(email))
                return false;

            return await _context.Clientes.AnyAsync(c =>
                c.Activo &&
                c.EMail.ToLower() == email.Trim().ToLower() &&
                (!idExcluir.HasValue || c.IdCliente != idExcluir.Value));
        }
    }
}
